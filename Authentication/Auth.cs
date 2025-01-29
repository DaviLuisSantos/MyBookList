using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyBookList.Authentication;

    public static class Auth
    {
    public static void AddAuth(this WebApplicationBuilder builder)
    {
        string? secretKey = builder.Configuration["AppSettings:SecretKey"];
        string? authority = builder.Configuration["AppSettings:Authority"];
        string? audience = builder.Configuration["AppSettings:Audience"];
        string? issuer = builder.Configuration["AppSettings:Issuer"];
        if(string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(authority) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(issuer))
        {
            throw new ArgumentException("Missing required JWT configuration in appsettings.json");
        }
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        builder.Services.AddAuthorization();
    }
    }
