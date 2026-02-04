using CinemaLite.Infrastructure.Consumers.CacheInvalidation;
using CinemaLite.Infrastructure.MessageBroker;
using MassTransit;

namespace CinemaLite.Api.Extensions.MessageBroker;

public static class MassTransitConfiguration
{
    public static IServiceCollection AddMassTransitMessageBroker(this IServiceCollection services)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                var messageBrokerSettings = context.GetRequiredService<MessageBrokerSettings>();

                cfg.Host(messageBrokerSettings.Host, "/", h =>
                {
                    h.Username(messageBrokerSettings.Username);
                    h.Password(messageBrokerSettings.Password);
                });
                
                cfg.ReceiveEndpoint("movie-cache-invalidation-queue",e =>
                {
                    e.ConfigureConsumer<MovieCacheInvalidationConsumer>(context);
                });
                
                cfg.ConfigureEndpoints(context);
            });

            busConfigurator.AddConsumer<MovieCacheInvalidationConsumer>();
        });
        
        return services;
    }
}