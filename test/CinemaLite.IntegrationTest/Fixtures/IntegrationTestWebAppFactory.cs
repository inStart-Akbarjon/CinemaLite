using CinemaLite.Infrastructure.Database;
using CinemaLite.IntegrationTest.JwtTokenSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace CinemaLite.IntegrationTest.Fixtures;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder("redis:7-alpine")
        .WithPortBinding(6379, true)
        .Build();
    
    private readonly RabbitMqContainer _rabbitMqContainer =
        new RabbitMqBuilder("rabbitmq:3.13-management-alpine")
            .WithUsername("guest")
            .WithPassword("guest")
            .WithPortBinding(5672, true)
            .WithPortBinding(15672, true)
            .Build();

    private string? _postgresConnectionString;
    private string? _redisConnectionString;
    private string? _rabbitMqHost;
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureHostConfiguration(config =>
        {
            Dictionary<string, string?> settings = new()
            {
                // Connection settings
                ["ConnectionStrings:DefaultConnection"] = _postgresConnectionString,
                ["Redis:ConnectionString"] = _redisConnectionString,
                ["MessageBroker:Host"] = _rabbitMqContainer.Hostname,
                ["MessageBroker:Port"] = _rabbitMqContainer.GetMappedPublicPort(5672).ToString(),
                ["MessageBroker:Username"] = "guest",
                ["MessageBroker:Password"] = "guest",
                
                // Jwt settings
                ["AuthSettings:SecretKey"] =  JwtTokenTestSettings.SecretKey,
                ["AuthSettings:Audience"] = JwtTokenTestSettings.Audience,
                ["AuthSettings:Issues"] = JwtTokenTestSettings.Issuer,
                ["AuthSettings:ExpireTimeInSeconds"] = JwtTokenTestSettings.ExpireTimeInSeconds.ToString(),
            };

            config.AddInMemoryCollection(settings);
        });
        
        return base.CreateHost(builder);
    }
    
    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        _postgresConnectionString = _postgresContainer.GetConnectionString();

        await _redisContainer.StartAsync();
        _redisConnectionString = _redisContainer.GetConnectionString();
        
        await _rabbitMqContainer.StartAsync();
        _rabbitMqHost =
            $"{_rabbitMqContainer.Hostname}:{_rabbitMqContainer.GetMappedPublicPort(5672)}";
        
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
    }
}
