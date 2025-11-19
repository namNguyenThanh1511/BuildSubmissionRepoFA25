using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE181580.api.Extensions {
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddAuthenticationConfig(this IServiceCollection service, IConfiguration configuration) //MAY RENAME OR RELOCATE LATER
        {
            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]
                ?? throw new Exception("Missing Jwt:Key")));
            service.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // No Issuer validation
                    ValidateAudience = false, // No Audience validation
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtKey,
                    RoleClaimType = ClaimTypes.Role
                };
            });                
            return service;
        }
    }
}
