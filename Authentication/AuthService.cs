using MyBookList.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace MyBookList.Authentication;

public static class AuthService
{
    private static readonly IConfiguration _configuration;
    static AuthService()
    {
        // Carrega as configurações do appsettings.json
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();
    }

    public static string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        string? secretKey = _configuration["AppSettings:SecretKey"];
        string? audience = _configuration["AppSettings:Audience"];
        var issuer = _configuration["AppSettings:Issuer"];

        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "role")
        }),
            Expires = DateTime.UtcNow.AddHours(2),
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
