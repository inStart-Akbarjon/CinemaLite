using CinemaLite.Application.CQRS.Auth.Register.Validators;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Mappers;
using CinemaLite.Application.Services.Implementations.Auth;
using CinemaLite.Application.Services.Interfaces.Auth;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaLite.Application.Extensions.Common;

public static class DependencyInjectionRegistrationExtension
{
    public static IServiceCollection AddDependencyInjectionRegistrationService(this IServiceCollection services)
    {
        services.AddScoped<IAuthMapper, AuthMapper>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        services.AddValidatorsFromAssembly(typeof(RegisterCommandValidator).Assembly);
        
        return services;
    }
}