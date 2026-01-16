using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Application.CQRS.Movie.Commands.DeleteMovie;

public record DeleteMovieCommand(Guid Id) : IRequest<IActionResult>
{
    
}