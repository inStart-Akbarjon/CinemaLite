using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Services.Interfaces.Auth;

public interface IPasswordHasherService
{
    public string HashPassword(ApplicationUser user, string password);
    public bool VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword);
}