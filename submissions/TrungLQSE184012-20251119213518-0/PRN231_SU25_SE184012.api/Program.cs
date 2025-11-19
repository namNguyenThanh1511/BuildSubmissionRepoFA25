
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repo.Base;
using Repo.Models;
using Service;
using Service.Helper;
using System.Text;
using System.Text.Json;

namespace PRN231_SU25_SE184012.api
{
    public class Program
    {
        public static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<LeopardAccount>(nameof(LeopardAccount));
            odataBuilder.EntitySet<LeopardType>(nameof(LeopardType));
            odataBuilder.EntitySet<LeopardProfile>(nameof(LeopardProfile));

            return odataBuilder.GetEdmModel();
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder
                .Services.AddControllers()
                .AddOData(options =>
                    options
                        .Select()
                        .Filter()
                        .OrderBy()
                        .Expand()
                        .Count()
                        .SetMaxTop(100)
                        .AddRouteComponents("odata", GetEdmModel())
                )
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System
                        .Text
                        .Json
                        .Serialization
                        .ReferenceHandler
                        .IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            builder
                .Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var errorResponse = new ErrorModel(
                                ConstantsConfig.ErrorMessages.Unauthorized,
                                ConstantsConfig.ErrorCodes.Unauthorized
                            );
                            var json = JsonSerializer.Serialize(errorResponse);
                            context.HandleResponse();
                            return context.Response.WriteAsync(json);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var errorResponse = new ErrorModel(
                                ConstantsConfig.ErrorMessages.PermissionDenied,
                                ConstantsConfig.ErrorCodes.PermissionDenied
                            );
                            var json = JsonSerializer.Serialize(errorResponse);
                            return context.Response.WriteAsync(json);
                        },
                    };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JwtToken:Key"])
                        ),
                    };
                });

            // Register Helpers
            builder.Services.AddScoped(typeof(GenericRepository<>));
            builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<LeopardProfileService>();
            builder.Services.AddScoped<LeopardTypeService>();
            
            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.DescribeAllParametersInCamelCase();
                option.ResolveConflictingActions(conf => conf.First());
                option.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer",
                    }
                );
                option.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                            },
                            new string[] { }
                        },
                    }
                );
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
        }
    }
}
