using CinemaLite.Application.CQRS.Session.Commands.CreateSession;
using CinemaLite.Application.CQRS.Session.Commands.DeleteSession;
using CinemaLite.Application.CQRS.Session.Commands.UpdateSession;
using CinemaLite.Application.CQRS.Session.Queries.GetAllSessions;
using CinemaLite.Application.CQRS.Session.Queries.GetSessionById;
using CinemaLite.Application.DTOs.Session.Request;
using CinemaLite.Application.DTOs.Session.Respone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace CinemaLite.Api.Controllers;

[ApiController]
[Route("api/movies")]
public class SessionController(IMediator mediator) : ControllerBase
{
    [HttpGet("{movieId}/sessions")]
    public async Task<GetAllSessionsFromMovieResponse> GetMovieSessions([FromRoute] Guid movieId, CancellationToken cancellationToken) 
    {
        var query = new GetAllSessionsQuery(movieId);
        return await mediator.Send(query, cancellationToken);
    }
    
    [HttpGet("{movieId}/sessions/{id}")]
    public async Task<GetSessionByIdResponse> GetSessionById([FromRoute] Guid movieId, [FromRoute] Guid id, CancellationToken cancellationToken) 
    {
        var query = new GetSessionByIdQuery(id, movieId);
        return await mediator.Send(query, cancellationToken);
    }
    
    [Authorize]
    [HttpPost("{movieId}/sessions")]
    public async Task<CreateSessionResponse> CreateSession([FromRoute] Guid movieId, [FromBody] CreateSessionRequest request, CancellationToken cancellationToken) 
    {
        var command = new CreateSessionCommand(movieId, request.CinemaName, request.AvailableSeats, request.Price, request.StartTime);
        return await mediator.Send(command, cancellationToken);
    }
    
    [Authorize]
    [HttpPut("{movieId}/sessions/{id}")]
    public async Task<UpdateSessionResponse> UpdateSession([FromRoute] Guid movieId, [FromRoute] Guid id, [FromBody] UpdateSessionRequest request, CancellationToken cancellationToken) 
    {
        var command = new UpdateSessionCommand(
            id,
            movieId,
            request.CinemaName,
            request.Price, 
            request.AvailableSeats, 
            request.StartTime
        );
        
        return await mediator.Send(command, cancellationToken);
    }
    
    [Authorize]
    [HttpDelete("{movieId}/sessions/{id}")]
    public async Task<IActionResult> DeleteSession([FromRoute] Guid movieId, [FromRoute] Guid id, CancellationToken cancellationToken) 
    {
        var command = new DeleteSessionCommand(id, movieId);
        return await mediator.Send(command, cancellationToken);
    }
}