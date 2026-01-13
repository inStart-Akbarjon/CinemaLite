using CinemaLite.Application.DTOs.Auth.Registration.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Auth.Register.Commands;

public record RegisterCommand(    
    string Email,
    string Password
) : IRequest<RegisterApplicationUserResponse>
{
}