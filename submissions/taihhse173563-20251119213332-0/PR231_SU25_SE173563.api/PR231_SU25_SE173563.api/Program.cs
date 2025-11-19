using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Models;
using Service;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Su25leopardDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<LeopardProfile>("LeopardProfile/search");
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    })
    .AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100).AddRouteComponents(
        "api",
        modelBuilder.GetEdmModel()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddScoped<LeopardService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HaHuyTai",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
var key = builder.Configuration.GetValue<string>("JwtSettings:SecretKey");
var issurer = builder.Configuration.GetValue<string>("JwtSettings:Issuer");
var audience = builder.Configuration.GetValue<string>("JwtSettings:Audience");
builder.Services.AddAuthentication(x =>
{
x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
x.RequireHttpsMetadata = false;
x.SaveToken = true;
x.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidIssuer = issurer,
    ValidAudience = audience
};
x.Events = new JwtBearerEvents
{
    OnChallenge = context =>
    {
        // Prevent the default response
        context.HandleResponse();

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var response = new
        {
            code = "HB40101",
            message = "Token missing/invalid"
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    },
    OnForbidden = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";

        var response = new
        {
            code = "HB40301",
            message = "Permission denied"
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
};
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
