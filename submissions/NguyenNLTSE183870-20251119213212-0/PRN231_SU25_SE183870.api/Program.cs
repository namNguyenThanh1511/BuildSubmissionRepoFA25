using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using Service;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Odata
builder
    .Services.AddControllers()
    .AddOData(options =>
    {
        options.Select().Filter().OrderBy().Expand();
    });
// Berer Token Authentication
builder
    .Services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
    .AddJwtBearer(
        BearerTokenDefaults.AuthenticationScheme,
        options =>
        {
            options.TokenValidationParameters =
                new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                    ),
                };

            options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
            {
                OnChallenge = context =>
                {
                  
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(
                        new ErrorResponse("HB40101", "Token missing or invalid"),
                        new JsonSerializerOptions()
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        }
                    );

                    return context.Response.WriteAsync(result);
                },

                OnForbidden = context =>
                {
                   
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(
                        new ErrorResponse("HB40301", "Permission denied"),
                        new JsonSerializerOptions()
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        }
                    );

                    return context.Response.WriteAsync(result);
                },
            };
        }
    );
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Token only (no need to type 'Bearer ')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });


    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<Repository.AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<Repository.LeopardProfileRepository>();
builder.Services.AddScoped<ILeopardProfileService, LeopardProfileService>();

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
