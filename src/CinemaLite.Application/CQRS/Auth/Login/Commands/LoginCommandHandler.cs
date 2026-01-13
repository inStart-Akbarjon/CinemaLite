using CinemaLite.Application.DTOs.Auth.Login.Respone;
using CinemaLite.Application.Exceptions.Auth;
using CinemaLite.Application.Exceptions.Validation;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace CinemaLite.Application.CQRS.Auth.Login.Commands;

public class LoginCommandHandler(
    IJwtTokenGeneratorService jwtTokenGeneratorService,
    IPasswordHasherService passwordHasherService,
    IAuthMapper authMapper,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    IValidator<LoginCommand> validator
) : IRequestHandler<LoginCommand, LoginApplicationUserResponse>
{
    public async Task<LoginApplicationUserResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            foreach (var failure in validationResult.Errors)
            {
                throw new InvalidRequestException(failure.ErrorMessage);
            }
        }
        
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
        
        var accessToken = jwtTokenGeneratorService.GenerateJwtToken(user, "Customer");
        
        var expiresIn = configuration.GetValue<int>("AuthSettings:ExpireTimeInSeconds");
        
        return authMapper.ToLoginApplicationUserResponse(accessToken, expiresIn);
    }
}