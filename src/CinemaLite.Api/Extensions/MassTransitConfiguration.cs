using CinemaLite.Infrastructure.Consumers;
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
                
                cfg.ReceiveEndpoint("reservation-expire-queue", e =>
                {
                    e.Bind("reservation.expire.exchange", x =>
                    {
                        x.RoutingKey = "expire-reservation";
                        x.ExchangeType = "direct";
                    });

                    e.ConfigureConsumer<CartExpireConsumer>(context);
                    e.ConfigureConsumer<OrderExpireConsumer>(context);
                    e.ConfigureConsumer<PaymentTransactionExpireConsumer>(context);
                });
            });
            
            busConfigurator.AddConsumer<CartExpireConsumer>();
            busConfigurator.AddConsumer<OrderExpireConsumer>();
            busConfigurator.AddConsumer<PaymentTransactionExpireConsumer>();
        });
        
        return services;
    }
}