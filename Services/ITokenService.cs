using Microsoft.IdentityModel.Tokens;
using MyBookList.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyBookList.Services
{
    public interface ITokenService
    {
        string GenerateToken(string user);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string username)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"] ?? "askjhdgjahgsdkhsadfhgjkbhsadjkfhbjadsffgadsdsfa");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("UserUuid", username) }),
                Expires = DateTime.UtcNow.AddHours(1), // Tempo de expiração do token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}