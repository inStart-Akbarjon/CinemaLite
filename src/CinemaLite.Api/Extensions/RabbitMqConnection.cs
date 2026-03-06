using CinemaLite.Infrastructure.MassageBroker;
using Microsoft.Extensions.Options;

namespace CinemaLite.Api.Extensions;

public static class RabbitMqConnection
{
    public static WebApplicationBuilder AddRabbitMq(
        this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
        
        builder.Services.AddMassTransitServices();
        
        return builder;
    }
}