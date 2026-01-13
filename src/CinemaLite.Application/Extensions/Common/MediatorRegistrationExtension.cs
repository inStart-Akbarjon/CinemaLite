using CinemaLite.Application.CQRS.Auth.Register.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaLite.Application.Extensions.Common;

public static class MediatorRegistrationExtension
{
    public static IServiceCollection AddMediatorRegistrationService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
        });
        
        return services;
    }
}