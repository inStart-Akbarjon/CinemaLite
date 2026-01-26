using CinemaLite.Application.CQRS.Ticket.Command.CreateTicket;
using CinemaLite.Application.CQRS.Ticket.Queries.GetUserTickets;
using CinemaLite.Application.DTOs.Ticket.Request;
using CinemaLite.Application.DTOs.Ticket.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace CinemaLite.Api.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("my")]
    public async Task<GetUserTicketsResponse> GetUserTickets(CancellationToken cancellationToken) 
    {
        var query = new GetUserTicketsQuery();
        return await mediator.Send(query, cancellationToken);
    }

    [Authorize]
    [HttpPost("{movieId}/sessions/{sessionId}")]
    public async Task<CreateTicketResponse> CreateTicket([FromRoute] Guid movieId, [FromRoute] Guid sessionId, [FromBody] CreateTicketRequest request, CancellationToken cancellationToken) 
    {
        var command = new CreateTicketCommand(
            movieId,
            sessionId,
            request.SeatRow, 
            request.SeatNumber);
        
        return await mediator.Send(command, cancellationToken);
    }
}