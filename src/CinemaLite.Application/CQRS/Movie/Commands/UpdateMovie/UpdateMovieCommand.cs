using CinemaLite.Application.DTOs.Movie.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Movie.Commands.UpdateMovie;

public record UpdateMovieCommand(  
    Guid Id,
    string Title,
    int DurationMinutes,
    string Genre
) : IRequest<UpdateMovieResponse>
{
    
}