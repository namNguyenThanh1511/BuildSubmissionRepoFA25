
using BusinessLogic.Service;
using DataAccess.Context;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Common;
using Repositories.Mapping;
using System.Text;
using System.Text.Json;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();

            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<LeopardProfileService>();
            builder.Services.AddScoped<LeopardAccountRepositories>();
            builder.Services.AddScoped<LeopardProfileRepositories>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddDbContext<Su25leopardDbContext>();
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
                    x.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var result = JsonSerializer.Serialize(new
                            {
                                errorCode = ErrorHelper.Unauthorized("missing tokens"),
                            });

                            return context.Response.WriteAsync(result);
                        },

                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";

                            var result = JsonSerializer.Serialize(new
                            {
                                errorCode = ErrorHelper.Forbidden("Permission Denied")
                            });

                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            // Add Swagger JWT configuration
            builder.Services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Chỉ dán mỗi token, không cần thêm 'Bearer'",
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
                options.AddPolicy("admin_or_mod", policyBuilder =>
                    policyBuilder.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Role")
                        && (context.User.FindFirst(c => c.Type == "Role").Value == "5"
                        || context.User.FindFirst(c => c.Type == "Role").Value == "6")));
                options.AddPolicy("all", policyBuilder =>
                    policyBuilder.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Role")
                        && (context.User.FindFirst(c => c.Type == "Role").Value == "4"
                        || context.User.FindFirst(c => c.Type == "Role").Value == "5"
                        || context.User.FindFirst(c => c.Type == "Role").Value == "6"
                        || context.User.FindFirst(c => c.Type == "Role").Value == "7")));
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
