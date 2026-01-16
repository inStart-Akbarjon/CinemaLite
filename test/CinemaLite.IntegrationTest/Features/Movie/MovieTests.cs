using System.Net.Http.Headers;
using System.Net.Http.Json;
using CinemaLite.Application.CQRS.Movie.Commands.CreateMovie;
using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Domain.Enums;
using CinemaLite.IntegrationTest.Base;
using CinemaLite.IntegrationTest.Fixtures;
using Shouldly;

namespace CinemaLite.IntegrationTest.Features.Movie;

public class MovieTests(IntegrationTestWebAppFactory factory) : BaseTest(factory)
{
    [Fact]
    public async Task CreateMovie_ReturnsCreatedMovie()
    {
        // Arrange
        var token = await GetJwtToken(nameof(UserRole.Customer));

        var createCommand = new CreateMovieCommand("Spider Man", 120, "Action");
        
        // Act
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await Client.PostAsJsonAsync("/api/movie", createCommand);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<CreateMovieResponse>();
        matchResponse.ShouldNotBeNull();
        matchResponse.Id.ShouldNotBe(Guid.Empty);
    }
}