using CinemaLite.Application.CQRS.Auth.Login.Validators;
using CinemaLite.Application.CQRS.Auth.Register.Commands;
using CinemaLite.Application.CQRS.Auth.Register.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaLite.Application.Extensions.Common;

public static class MediatorRegistrationExtension
{
    public static WebApplicationBuilder AddMediatorRegistrationService(this WebApplicationBuilder builder)
    {
        // Mediator
        builder.Services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
        });
        
        // Validators
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(typeof(RegisterCommandValidator).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);
        
        return builder;
    }
}