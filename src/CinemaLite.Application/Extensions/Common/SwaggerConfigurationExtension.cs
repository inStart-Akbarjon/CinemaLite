using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace CinemaLite.Application.Extensions.Common;

public static class SwaggerConfigurationExtension
{
    public static IServiceCollection AddSwaggerConfiguration(
        this IServiceCollection services
    )
    {
        services.AddSwaggerGen(options =>
        {
            const string schemeId = "Bearer";

            options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "Enter only JWT token (without Bearer keyword)",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        }); 
            
        return services;
    }
}