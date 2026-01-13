using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CinemaLite.Infrastructure.Database.Extensions;

public static class DatabaseConnectionExtension
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAppDbContext, AppDbContext>();
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(
            configuration.GetConnectionString("DefaultConnection")
        );

        dataSourceBuilder.EnableDynamicJson();

        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });
        
        services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }
}