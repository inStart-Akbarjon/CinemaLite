using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Mappers;
using CinemaLite.Application.Services.Implementations.Auth;
using CinemaLite.Application.Services.Implementations.Common;
using CinemaLite.Application.Services.Implementations.RedisDistributedCache;
using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Application.Services.Interfaces.Common;
using CinemaLite.Application.Services.Interfaces.RedisDistributedCache;
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
        builder.Services.AddScoped<ICartMapper, CartMapper>();
        builder.Services.AddScoped<IOrderMapper, OrderMapper>();
        builder.Services.AddScoped<ISeatReservationMapper, SeatReservationMapper>();
        builder.Services.AddScoped<IOpenSeatsStatus, OpenSeatsStatus>();
            
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.AddScoped<IMovieCacheService, MovieCacheService>();
        builder.Services.AddScoped<ITopMovieCacheService, TopMovieCacheService>();
        
        return builder;
    }
}