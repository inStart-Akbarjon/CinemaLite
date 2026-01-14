using CinemaLite.Application.CQRS.Auth.Register.Commands;
using CinemaLite.Application.DTOs.Auth.Login.Respone;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Interfaces.Mappers;

public interface IAuthMapper
{
    public ApplicationUser ToApplicationUserEntity(RegisterCommand user);
    public LoginApplicationUserResponse ToLoginApplicationUserResponse(string accessToken, int expiresIn);
}