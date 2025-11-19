using BLL;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;


namespace PRN231_SU25_SE181526.api
{
    public static class ServiceCollectionExtension 
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Su25leopardDbContext>
            (
              option => option.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"])
            );
        }
        public static void AddDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<AccountService>();

        }

        public static void AddAuthen(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
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

                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                context.Response.ContentType = "application/json";

                                var result = JsonSerializer.Serialize(new
                                {
                                    errorCode = "HB40101",
                                    message = "Token missing or invalid"
                                });

                                return context.Response.WriteAsync(result);
                            }
                        };
                    });


            services.AddAuthorization();

        }
    }
}
