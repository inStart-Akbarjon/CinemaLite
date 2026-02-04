using CinemaLite.Infrastructure.MessageBroker;
using Microsoft.Extensions.Options;

namespace CinemaLite.Api.Extensions.MessageBroker;

public static class RabbitMqConnection
{
    public static WebApplicationBuilder AddRabbitMqConnection(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
        
        builder.Services.AddMassTransitMessageBroker();
        
        return builder;
    }
}