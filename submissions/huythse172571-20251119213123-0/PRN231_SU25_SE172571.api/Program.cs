
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text;
using BOs;
using Microsoft.AspNetCore.OData;
using Repos;
using Services;

namespace PRN231_SU25_SE172571.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

         
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



            IConfiguration configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", true, true).Build();


           
            builder.Services.AddScoped<IAccountRepo, AccountRepo>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ILeopardProfileRepo, LeopardProfileRepo>();
            builder.Services.AddScoped<ILeopardProfileService, LeopardProfileService>();

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
                });

            
            builder.Services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter your JWT token in this field",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
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

                options.AddPolicy(
                     "ReadOnly",
        policyBuilder => policyBuilder.RequireAssertion(
                       context => context.User.HasClaim(claim => claim.Type == "Role")
                                  && (context.User.FindFirst(claim => claim.Type == "Role").Value == "5"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "6")));


                options.AddPolicy(
         "Full",
         policyBuilder => policyBuilder.RequireAssertion(
                        context => context.User.HasClaim(claim => claim.Type == "Role")
                                   && (context.User.FindFirst(claim => claim.Type == "Role").Value == "4"
                                              || context.User.FindFirst(claim => claim.Type == "Role").Value == "5"
                                              || context.User.FindFirst(claim => claim.Type == "Role").Value == "6"
                                              || context.User.FindFirst(claim => claim.Type == "Role").Value == "7"
                                               
                                              ))
         );
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            var app = builder.Build();
            app.UseRouting();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
