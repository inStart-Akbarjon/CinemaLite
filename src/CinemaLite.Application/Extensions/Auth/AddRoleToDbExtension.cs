using CinemaLite.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaLite.Application.Extensions.Auth;

public static class AddRoleToDbExtension
{
    public static async Task<WebApplication> AddRolesToDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        
        if (!await roleManager.RoleExistsAsync(nameof(UserRole.Admin)))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(nameof(UserRole.Admin)));
        }

        if (!await roleManager.RoleExistsAsync(nameof(UserRole.Customer)))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(nameof(UserRole.Customer)));
        }
        
        return app;
    }
}