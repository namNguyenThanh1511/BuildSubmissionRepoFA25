using BLL;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<LeopardProfileService>();
builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddDbContext<Su25leopardDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<JWTSettings>(
    builder.Configuration.GetSection("JWTSettings"));
var key = builder.Configuration.GetValue<string>("JwtSettings:SecretKey");
var issurer = builder.Configuration.GetValue<string>("JwtSettings:Issuer");
var audience = builder.Configuration.GetValue<string>("JwtSettings:Audience");
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Paste your JWT token below. No need to type 'Bearer'.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, // Important: set this to Http
        Scheme = "bearer", // Important: use lowercase 'bearer'
        BearerFormat = "JWT" // Optional, for display purposes
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
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
    x.Authority = "Authority URL"; // TODO: Update URL
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
