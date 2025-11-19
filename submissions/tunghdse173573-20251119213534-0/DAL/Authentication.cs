using DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Authentication
    {
        public static async Task<LoginResponseDTO> CreateToken(LeopardAccount user, string? role, JWTSettings jwtSettings, bool isRefresh = false)
        {
            // Tạo ra các claims
            DateTime now = DateTime.Now;

            // Danh sách các claims chung cho cả Access Token và Refresh Token
            List<Claim> claims = new List<Claim>
         {
             new Claim("id", user!.AccountId.ToString()),
             new Claim("role", role.ToString()),
             new Claim("email",user.Email),
         };

            // đăng kí khóa bảo mật
            SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey ?? string.Empty));
            SigningCredentials? creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            // Generate access token
            DateTime dateTimeAccessExpr = now.AddDays(jwtSettings.AccessTokenExpirationMinutes);
            claims.Add(new Claim("token_type", "access"));
            JwtSecurityToken accessToken = new JwtSecurityToken(
                claims: claims,
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                expires: dateTimeAccessExpr,
                signingCredentials: creds
            );

            string refreshTokenString = string.Empty;
            string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            if (isRefresh == false)
            {
                // tạo ra refresh Token
                DateTime datetimeRefrestExpr = now.AddDays(jwtSettings.RefreshTokenExpirationDays);

                claims.Remove(claims.First(c => c.Type == "token_type"));
                claims.Add(new Claim("token_type", "refresh"));

                JwtSecurityToken? refreshToken = new JwtSecurityToken(
                    claims: claims,
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    expires: datetimeRefrestExpr,
                    signingCredentials: creds
                );

                refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);
            }

            return new LoginResponseDTO
            {
                Role = role,
                Token = accessTokenString,
            };
        }
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "id");
            return Guid.TryParse(userIdClaim?.Value, out Guid userId) ? userId : Guid.Empty;
        }
        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x =>
                x.Type.Equals(ClaimTypes.Email) ||
                x.Type.Equals("email") ||
                x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))
                ?.Value ?? string.Empty;
        }
    }
}
