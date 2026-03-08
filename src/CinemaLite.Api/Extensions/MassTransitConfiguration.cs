using CinemaLite.Infrastructure.MassageBroker;
using MassTransit;

namespace CinemaLite.Api.Extensions;

public static class MassTransitConfiguration
{
    public static IServiceCollection AddMassTransitServices(this IServiceCollection services)
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
            });
        });
        
        return services;
    }
}