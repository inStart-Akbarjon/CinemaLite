using CinemaLite.Domain.Models;
using CinemaLite.Infrastructure.Database;
using CinemaLite.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using CinemaLite.Application.Services.Interfaces.Auth;

namespace CinemaLite.IntegrationTest.Base;

public class BaseTest : IAsyncLifetime, IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient Client;
    protected readonly IntegrationTestWebAppFactory Factory;

    protected BaseTest(IntegrationTestWebAppFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    protected async Task<string> GetJwtToken(string role)
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var tokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenGeneratorService>();

        var user = new ApplicationUser
        {
            UserName = "test_user",
            Email = $"test_email@test.com"
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return tokenService.GenerateJwtToken(user, role);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}