using CinemaLite.Application.CQRS.Movie.Commands.CreateMovie;
using CinemaLite.Application.CQRS.Movie.Commands.DeleteMovie;
using CinemaLite.Application.CQRS.Movie.Commands.UpdateMovie;
using CinemaLite.Application.CQRS.Movie.Queries.GetAllMovies;
using CinemaLite.Application.CQRS.Movie.Queries.GetMovieById;
using CinemaLite.Application.DTOs.Movie.Request;
using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace CinemaLite.Api.Controllers;

[ApiController]
[Route("api/movie")]
public class MovieController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<PaginatedMovieList<GetAllMoviesResponse>> GetAllMovies(
        [FromQuery] GetAllMoviesQuery request, 
        CancellationToken cancellationToken
    ) {
        return await mediator.Send(request, cancellationToken);
    }
    
    [HttpGet("{id}")]
    public async Task<GetMovieByIdResponse> GetMovieById(
        [FromRoute] GetMovieByIdQuery request, 
        CancellationToken cancellationToken
    ) {
        return await mediator.Send(request, cancellationToken);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<CreateMovieResponse> CreateMovie(
        [FromBody] CreateMovieCommand request, 
        CancellationToken cancellationToken
    ) {
        return await mediator.Send(request, cancellationToken);
    }
    
    [Authorize]
    [HttpPut("{id}")]
    public async Task<UpdateMovieResponse> UpdateMovie(
        [FromRoute] Guid id, 
        [FromBody] UpdateMovieRequest request,
        CancellationToken cancellationToken
    ) {
        var command = new UpdateMovieCommand(
            id,
            request.Title,
            request.DurationMinutes,
            request.Genre);
        
        return await mediator.Send(command, cancellationToken);
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(
        [FromRoute] DeleteMovieCommand request, 
        CancellationToken cancellationToken
    ) {
        return await mediator.Send(request, cancellationToken);
    }
}