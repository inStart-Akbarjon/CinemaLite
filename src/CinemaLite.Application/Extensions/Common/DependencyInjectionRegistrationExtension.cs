using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Mappers;
using CinemaLite.Application.Services.Implementations.Auth;
using CinemaLite.Application.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaLite.Application.Extensions.Common;

public static class DependencyInjectionRegistrationExtension
{
    public static WebApplicationBuilder AddDependencyInjectionRegistrationService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthMapper, AuthMapper>();
        builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        builder.Services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        builder.Services.AddScoped<IMovieMapper, MovieMapper>();
        builder.Services.AddScoped<ISessionMapper, SessionMapper>();
        builder.Services.AddScoped<ITicketMapper, TicketMapper>();
            
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        return builder;
    }
}