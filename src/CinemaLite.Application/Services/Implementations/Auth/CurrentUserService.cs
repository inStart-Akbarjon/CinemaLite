using CinemaLite.Application.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Services.Implementations.Auth;

public class CurrentUserService : ICurrentUserService
{
    public int UserId { get; }

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        var user = accessor.HttpContext?.User;
        
        var id = user?.FindFirst("id")?.Value;
        
        if (id == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (!int.TryParse(id, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid UserId claim");
        }
        
        UserId = userId;
    }
}