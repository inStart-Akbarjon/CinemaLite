using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Services.Interfaces.Auth;

public interface IJwtTokenGeneratorService
{
    public string GenerateJwtToken(ApplicationUser user, string role);
}