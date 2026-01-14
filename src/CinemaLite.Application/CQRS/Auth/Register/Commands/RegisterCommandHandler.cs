using CinemaLite.Application.Exceptions.Auth;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CinemaLite.Domain.Models;
using MediatR;

namespace CinemaLite.Application.CQRS.Auth.Register.Commands;

public class RegisterCommandHandler(
    IPasswordHasherService passwordHasherService,
    IAuthMapper authMapper,
    UserManager<ApplicationUser> userManager
) : IRequestHandler<RegisterCommand, IActionResult>
{
    public async Task<IActionResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var duplicateUser = await userManager.FindByEmailAsync(request.Email);
        
        if (duplicateUser != null)
        {
            throw new DuplicateUserEmailException($"{request.Email}");
        }
        
        var applicationUserEntity = authMapper.ToApplicationUserEntity(request);
        
        var hashedPassword = passwordHasherService.HashPassword(applicationUserEntity, request.Password);
        
        applicationUserEntity.PasswordHash = hashedPassword;
        
        await userManager.CreateAsync(applicationUserEntity);
        
        await userManager.AddToRoleAsync(applicationUserEntity, nameof(UserRole.Customer));

        return new OkResult();
    }
}