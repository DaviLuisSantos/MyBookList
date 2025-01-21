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
        string GenerateToken(User user);
    }

    public class TokenService : ITokenService
    {
        private readonly byte[] _key;

        public TokenService(byte[] key)
        {
            /*
            if (key.Length < 32)
            {
                // Gerar uma nova chave de 256 bits (32 bytes)
                key = new byte[32];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(key);
                }
            }
            */
            _key = key;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}