using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CinemaLite.Application.Extensions.Auth;

public static class AuthJwtBearerConfigurationExtension
{
    public static IServiceCollection AddJwtBearerConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(oprions =>
            {
                oprions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetValue<string>("AuthSettings:Issuer"),
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("AuthSettings:Audience"),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetValue<string>("AuthSettings:SecretKey")!)
                    )
                };
            });

        return services;
    }
}