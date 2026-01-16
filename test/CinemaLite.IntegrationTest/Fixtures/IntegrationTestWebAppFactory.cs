using CinemaLite.Infrastructure.Database;
using CinemaLite.IntegrationTest.JwtTokenSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace CinemaLite.IntegrationTest.Fixtures;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private string? _connectionString;
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureHostConfiguration(config =>
        {
            Dictionary<string, string?> settings = new()
            {
                // Connection settings
                ["ConnectionStrings:DefaultConnection"] = _connectionString,
                
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
        _connectionString = _postgresContainer.GetConnectionString();
        
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
    }
}
