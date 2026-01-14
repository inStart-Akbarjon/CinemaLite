using CinemaLite.Application.DTOs.Auth.Login.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Auth.Login.Commands;

public record LoginCommand(    
    string Email,
    string Password
) : IRequest<LoginApplicationUserResponse>
{
}