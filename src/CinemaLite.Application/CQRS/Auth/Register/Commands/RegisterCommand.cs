using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Application.CQRS.Auth.Register.Commands;

public record RegisterCommand(    
    string Email,
    string Password
) : IRequest<IActionResult>
{
}