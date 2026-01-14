using CinemaLite.Application.DTOs.Auth.Login.Respone;
using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Exceptions.Auth;
using CinemaLite.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using CinemaLite.Domain.Models;
using MediatR;

namespace CinemaLite.Application.CQRS.Auth.Login.Commands;

public class LoginCommandHandler(
    IJwtTokenGeneratorService jwtTokenGeneratorService,
    IPasswordHasherService passwordHasherService,
    IAuthMapper authMapper,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration
) : IRequestHandler<LoginCommand, LoginApplicationUserResponse>
{
    public async Task<LoginApplicationUserResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
        {
            throw new NotFoundUserException($"{request.Email}");
        }
        
        var isValid = passwordHasherService.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
        
        if (!isValid)
        {
            throw new InValidPasswordException($"{request.Email}");
        }
        
        var accessToken = jwtTokenGeneratorService.GenerateJwtToken(user, nameof(UserRole.Customer));
        
        var expiresIn = configuration.GetValue<int>("AuthSettings:ExpireTimeInSeconds");
        
        return authMapper.ToLoginApplicationUserResponse(accessToken, expiresIn);
    }
}