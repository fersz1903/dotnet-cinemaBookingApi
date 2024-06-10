using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserAuthApi.Entites;
using UserAuthApi.Models;

namespace UserAuthApi.Services{
    public static class TokenHelper
    {
        public static string GenerateToken(string email, IConfiguration configuration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            _ = int.TryParse(configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                },
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsPrincipal ValidateToken(string token, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        }

        // public static string GenerateLoginJwtToken(User user,IConfiguration configuration)
        // {
        //     var tokenHandler = new JwtSecurityTokenHandler();
        //     var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

        //     var tokenDescriptor = new SecurityTokenDescriptor
        //     {
        //         Subject = new ClaimsIdentity(new[] { new Claim("Email", user.Email.ToString()) }),
        //         Expires = DateTime.UtcNow.AddDays(7),
        //         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //     };

        //     var token = tokenHandler.CreateToken(tokenDescriptor);
        //     return tokenHandler.WriteToken(token);
        // }

        public static string GenerateLoginJwtToken(User user, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // User ID
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique ID for the token
                }),
                //Expires = DateTime.UtcNow.AddDays(7),
                Expires = DateTime.UtcNow.AddMinutes(1),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}