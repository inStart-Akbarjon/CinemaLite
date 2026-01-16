using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Application.CQRS.Session.Commands.DeleteSession;

public record DeleteSessionCommand(Guid Id, Guid MovieId) : IRequest<IActionResult>
{
    
}