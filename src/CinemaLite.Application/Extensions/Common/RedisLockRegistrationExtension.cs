using CinemaLite.Application.Services.Implementations.RedisDistributedLock;
using CinemaLite.Application.Services.Interfaces.RedisDistributedLock;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CinemaLite.Application.Extensions.Common;

public static class RedisLockRegistrationExtension
{
    public static WebApplicationBuilder AddRedisLock(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var config = ConfigurationOptions.Parse(builder.Configuration["Redis:ConnectionString"]!);

            config.AbortOnConnectFail = true;
            config.ConnectTimeout = 500;
            config.SyncTimeout = 500;
            config.ConnectRetry = 1;

            var multiplexer = ConnectionMultiplexer.Connect(config);
            
            return multiplexer;
        });

        builder.Services.AddSingleton<IDistributedLockService, DistributedLockService>();
        
        return builder;
    }
}