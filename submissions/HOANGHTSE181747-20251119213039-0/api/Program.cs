
using BLL.middleware;
using BLL.Service;
using DAL;
using DAL.repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
namespace api
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
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

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
                    "AdminDevMember",
                    policyBuilder => policyBuilder.RequireAssertion(
                        context => context.User.HasClaim(claim => claim.Type == "Role")
                        && ((context.User.FindFirst(claim => claim.Type == "Role").Value == "5")
                        || (context.User.FindFirst(claim => claim.Type == "Role").Value == "6")
                        || (context.User.FindFirst(claim => claim.Type == "Role").Value == "4")
                        || (context.User.FindFirst(claim => claim.Type == "Role").Value == "7")

                        )));

                options.AddPolicy(
                    "AdminOnly",
                    policyBuilder => policyBuilder.RequireAssertion(
                        context => context.User.HasClaim(claim => claim.Type == "Role")
                        && ((context.User.FindFirst(claim => claim.Type == "Role").Value == "5" || context.User.FindFirst(claim => claim.Type == "Role").Value == "6"))));

                options.AddPolicy(
                    "Developer",
                           policyBuilder => policyBuilder.RequireAssertion(
                                          context => context.User.HasClaim(claim => claim.Type == "Role")
                                                     && context.User.FindFirst(claim => claim.Type == "Role").Value == "7"));
                options.AddPolicy(
                    "Member",
                           policyBuilder => policyBuilder.RequireAssertion(
                                          context => context.User.HasClaim(claim => claim.Type == "Role")
                                                     && context.User.FindFirst(claim => claim.Type == "Role").Value == "4"));

            });



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IAccountRepo, AccountRepo>();
            builder.Services.AddScoped<ILeopardRepo, LeopardRepo>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ILeopardService, LeopardService>();


            var app = builder.Build();


            app.UseRouting();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.Run();
        }
    }
}
