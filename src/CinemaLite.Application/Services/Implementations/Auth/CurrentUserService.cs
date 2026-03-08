using System.Security.Claims;
using CinemaLite.Application.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Services.Implementations.Auth;

public class CurrentUserService : ICurrentUserService
{
    public int UserId { get; }
    public string UserPhone { get; }
    public string UserEmail { get; }

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        var user = accessor.HttpContext?.User;
        
        var id = user?.FindFirst("id")?.Value;
        var userEmail = user?.FindFirst(ClaimTypes.Email)?.Value;
        var phoneNumber = user?.FindFirst("phoneNumber")?.Value;
        
        if (id == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (!int.TryParse(id, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid UserId claim");
        }
        
        UserId = userId;
        UserPhone = phoneNumber;
        UserEmail = userEmail;
    }
}