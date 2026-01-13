using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace CinemaLite.Application.Services.Implementations.Auth;

public class PasswordHasherService : IPasswordHasherService
{
    public string HashPassword(ApplicationUser user, string password)
    {
        return new PasswordHasher<ApplicationUser>()
            .HashPassword(
                user,  
                password
            );
    }
    
    public bool VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
    {
        var result = new PasswordHasher<ApplicationUser>()
            .VerifyHashedPassword(
                user, 
                hashedPassword, 
                providedPassword
            );
    
        return result == PasswordVerificationResult.Success;
    }
}