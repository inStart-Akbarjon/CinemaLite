using CinemaLite.Application.DTOs.Auth.Registration.Respone;
using CinemaLite.Application.Exceptions.Auth;
using CinemaLite.Application.Exceptions.Validation;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CinemaLite.Application.CQRS.Auth.Register.Commands;

public class RegisterCommandHandler(
    IPasswordHasherService passwordHasherService,
    IAuthMapper authMapper,
    UserManager<ApplicationUser> userManager,
    IValidator<RegisterCommand> validator
) : IRequestHandler<RegisterCommand, RegisterApplicationUserResponse>
{
    public async Task<RegisterApplicationUserResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            foreach (var failure in validationResult.Errors)
            {
                throw new InvalidRequestException(failure.ErrorMessage);
            }
        }
        
        var duplicateUser = await userManager.FindByEmailAsync(request.Email);
        
        if (duplicateUser != null)
        {
            throw new DuplicateUserEmailException($"{request.Email}");
        }
        
        var applicationUserEntity = authMapper.ToApplicationUserEntity(request);
        
        var hashedPassword = passwordHasherService.HashPassword(applicationUserEntity, request.Password);
        
        applicationUserEntity.PasswordHash = hashedPassword;
        
        var result = await userManager.CreateAsync(applicationUserEntity);
        
        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join("; ", result.Errors.Select(e => e.Description)));
        
        await userManager.AddToRoleAsync(applicationUserEntity, "Customer");
        
        return authMapper.ToRegisterApplicationUserResponse(result.Succeeded);
    }
}