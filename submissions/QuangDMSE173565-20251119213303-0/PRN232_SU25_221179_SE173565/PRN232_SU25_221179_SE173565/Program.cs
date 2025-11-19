
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BLL;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using PRN232_SU25_221179_SE173565;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SU25LeopardDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});

builder.Services.AddScoped<AccountDAO>();
builder.Services.AddScoped<LeopardProfileDAO>();
builder.Services.AddScoped<IAccountBL, AccountBL>();
builder.Services.AddScoped<ILeopardProfileBL, LeopardProfileBL>();

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
})
.AddOData(opt =>
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<LeopardProfile>("LeopardProfiles");
    odataBuilder.EntitySet<LeopardType>("LeopardType");

    opt.AddRouteComponents("odata", odataBuilder.GetEdmModel())
     .Select().Filter().OrderBy().Count().Expand();
});


IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true, true).Build();

builder.Services.AddEndpointsApiExplorer();

builder.Services
.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidIssuer = configuration["JWT:Issuer"],
        ValidAudience = configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
    };

    // ? Chuy?n vào trong ?ây
    x.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new
            {
                errorCode = "FB40101",
                message = "Token missing or invalid"
            });
            return context.Response.WriteAsync(result);
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new
            {
                errorCode = "FB40301",
                message = "Permission denied"
            });
            return context.Response.WriteAsync(result);
        }

    };
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var errorResponse = new ErrorResponse
        {
            ErrorCode = "FB40001",
            Message = string.Join(" | ", errors)
        };

        var result = new ObjectResult(errorResponse)
        {
            StatusCode = StatusCodes.Status400BadRequest,
            DeclaredType = typeof(ErrorResponse)
        };

        return result;
    };
});


// Add Swagger JWT configuration
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "JWT Authentication for Cosmetics Management",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
};

    c.AddSecurityRequirement(securityRequirement);
});



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FullFunction", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Role") &&
             (context.User.FindFirst("Role")!.Value == "5" || context.User.FindFirst("Role")!.Value == "6")));
             

    options.AddPolicy("Search", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Role") &&
            new[] { "7", "4" }.Contains(context.User.FindFirst("Role")!.Value)));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();

