using CinemaLite.Infrastructure.Database;
using CinemaLite.IntegrationTest.JwtTokenSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
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

    private string? _postgresConnectionString;
    private string? _redisConnectionString;
    
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
        
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
    }
}
