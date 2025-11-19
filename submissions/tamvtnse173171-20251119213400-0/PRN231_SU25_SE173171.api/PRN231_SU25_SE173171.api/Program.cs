
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using PRN231_SU25_SE173171.BLL.Interfaces;
using PRN231_SU25_SE173171.BLL.Services;
using PRN231_SU25_SE173171.BLL.Store;
using PRN231_SU25_SE173171.DAL;
using PRN231_SU25_SE173171.DAL.Entities;
using PRN231_SU25_SE173171.DAL.Interfaces;
using PRN231_SU25_SE173171.DAL.Repositories;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PRN231_SU25_SE173171.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<LeopardProfile>("LeopardProfiles");

            builder.Services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                     options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
                 })
                 .AddOData(
                 options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
                     "odata",
                     modelBuilder.GetEdmModel()));
            builder.Services.AddControllers();
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            builder.Services.AddDbContext<Su25leopardDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MyDB")));
            builder.Services.AddScoped<ILeopardAccountRepository, LeopardAccountRepository>();
            builder.Services.AddScoped<ILeopardAccountService, LeopardAccountService>();
            builder.Services.AddScoped<ILeopardProfileRepository, LeopardProfileRepository>();
            builder.Services.AddScoped<ILeopardProfileService, LeopardProfileService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new
                        {
                            Field = e.Key,
                            Errors = e.Value.Errors.Select(x => x.ErrorMessage)
                        });

                    var result = new
                    {
                        errorCode = ErrorCode.ErrorCodeMissingInvalidInput,
                        message = errors
                    };

                    return new BadRequestObjectResult(result);
                };
            });

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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                        RoleClaimType = ClaimTypes.Role
                    };

                    x.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var result = JsonSerializer.Serialize(new
                            {
                                errorCode = ErrorCode.ErrorCodeTokenMissingInvalid,
                                message = "Invalid or expired token"
                            });

                            return context.Response.WriteAsync(result);
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                var result = JsonSerializer.Serialize(new
                                {
                                    errorCode = ErrorCode.ErrorCodeTokenMissingInvalid,
                                    message = "Authentication required"
                                });

                                return context.Response.WriteAsync(result);
                            }
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = 403;
                                context.Response.ContentType = "application/json";
                                var result = JsonSerializer.Serialize(new
                                {
                                    errorCode = ErrorCode.ErrorCodePermissionDenied,
                                    message = "You do not have permission to access this resource"
                                });

                                return context.Response.WriteAsync(result);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "JWT Authentication.",
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

            var app = builder.Build();

            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        errorCode = ErrorCode.ErrorCodeInternalServerError,
                        message = "Internal server error"
                    };

                    await context.Response.WriteAsJsonAsync(errorResponse);
                });
            });
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
        }
    }
}
