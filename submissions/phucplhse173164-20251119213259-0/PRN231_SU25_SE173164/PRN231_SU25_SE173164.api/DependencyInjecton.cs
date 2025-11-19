using PRN231_SU25_SE173164.dal;
using PRN231_SU25_SE173164.dal.Interfaces;
using PRN231_SU25_SE173164.dal.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using PRN231_SU25_SE173164.bll.Core;
using PRN231_SU25_SE173164.bll;

namespace PRN231_SU25_SE173164.api
{
    public static class DependencyInjecton
    {
        public static IServiceCollection AddConfigDI(this IServiceCollection services, IConfiguration configuration)
        {
            //DbContext
            services.AddDbContext<Su25leopardDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            //DI for Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            //DI for Services
            services.AddScoped<IService, Service>();
            services.AddScoped<JwtHelper>();


            //JWT Authentication
            var secret = configuration["JWT:Secret"]
                ?? throw new ArgumentNullException("JWT:Secret is missing in configuration");
            var issuer = configuration["JWT:ValidIssuer"]
                ?? throw new ArgumentNullException("JWT:ValidIssuer is missing in configuration");
            var audience = configuration["JWT:ValidAudience"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = !string.IsNullOrEmpty(audience),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse(); 
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new
                        {
                            errorCode = ErrorHelper.GetErrorCode(401),
                            message = ErrorHelper.GetErrorMessage(401)
                        });

                        return context.Response.WriteAsync(result);
                    },

                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new
                        {
                            errorCode = ErrorHelper.GetErrorCode(403),
                            message = ErrorHelper.GetErrorMessage(403)
                        });

                        return context.Response.WriteAsync(result);
                    }
                };
            });
            services.AddAuthorization();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PRN231_SU25_SE173164.api",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter JWT token: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
