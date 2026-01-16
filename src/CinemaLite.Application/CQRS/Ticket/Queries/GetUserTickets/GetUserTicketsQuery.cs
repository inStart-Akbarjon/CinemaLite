using CinemaLite.Application.DTOs.Ticket.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Ticket.Queries.GetUserTickets;

public record GetUserTicketsQuery() : IRequest<GetUserTicketsResponse> 
{
    
}