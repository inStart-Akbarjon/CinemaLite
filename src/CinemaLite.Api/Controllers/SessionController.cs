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
[Route("api/session")]
public class SessionController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<GetAllSessionsFromMovieResponse> GetMovieSessions([FromQuery] GetAllSessionsQuery request, CancellationToken cancellationToken) 
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<GetSessionByIdResponse> GetSessionById([FromRoute] Guid id, [FromBody] GetSessionByIdRequest request, CancellationToken cancellationToken) 
    {
        var query = new GetSessionByIdQuery(id, request.MovieId);
        
        return await mediator.Send(query, cancellationToken);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<CreateSessionResponse> CreateSession([FromBody] CreateSessionCommand request, CancellationToken cancellationToken) 
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    [HttpPut("{id}")]
    [Authorize]
    public async Task<UpdateSessionResponse> UpdateSession([FromRoute] Guid id, [FromBody] UpdateSessionRequest request, CancellationToken cancellationToken) 
    {
        var command = new UpdateSessionCommand(
            id,
            request.MovieId,
            request.CinemaName,
            request.Price, 
            request.AvailableSeats, 
            request.StartTime
        );
        
        return await mediator.Send(command, cancellationToken);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteSession([FromRoute] Guid id, [FromBody] Guid movieId, CancellationToken cancellationToken) 
    {
        var command = new DeleteSessionCommand(id, movieId);
        return await mediator.Send(command, cancellationToken);
    }
}