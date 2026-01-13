using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CinemaLite.Application.Services.Implementations.Auth;

public class JwtTokenGeneratorService(IConfiguration configuration) : IJwtTokenGeneratorService
{
    public string GenerateJwtToken(ApplicationUser user, string role)
    {
        var claims = new List<Claim>
        {
            new Claim("id",  user.Id.ToString()),
            new Claim("email",  user.Email),
            new Claim(ClaimTypes.Role, role)
        };

        var jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("AuthSettings:ExpireTimeInSeconds")), 
            claims: claims, 
            issuer: configuration.GetValue<string>("AuthSettings:Issuer"),
            audience: configuration.GetValue<string>("AuthSettings:Audience"),
            signingCredentials:
                new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetValue<string>("AuthSettings:SecretKey")!)
                    ),
           SecurityAlgorithms.HmacSha256
                )
        );
        
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}