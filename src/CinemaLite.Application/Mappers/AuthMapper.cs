using CinemaLite.Application.CQRS.Auth.Register.Commands;
using CinemaLite.Application.DTOs.Auth.Login.Respone;
using CinemaLite.Application.DTOs.Auth.Registration.Respone;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Mappers;

public class AuthMapper : IAuthMapper
{
    public ApplicationUser ToApplicationUserEntity(RegisterCommand user)
    {
        return new ApplicationUser()
        {
            Email = user.Email,
            UserName = user.Email
        };
    }

    public LoginApplicationUserResponse ToLoginApplicationUserResponse(string accessToken, int expiresIn)
    {
        return new LoginApplicationUserResponse()
        {
            AccessToken = accessToken,
            ExpiresIn = expiresIn,
        };
    }
    public RegisterApplicationUserResponse ToRegisterApplicationUserResponse(bool succeeded)
    {
        return new RegisterApplicationUserResponse()
        {
            Succeeded = succeeded
        };
    }
}