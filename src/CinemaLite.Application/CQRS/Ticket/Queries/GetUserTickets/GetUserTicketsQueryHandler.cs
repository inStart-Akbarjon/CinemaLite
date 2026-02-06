using CinemaLite.Application.DTOs.Ticket.Response;
using CinemaLite.Application.Exceptions.Ticket;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Services.Implementations.Auth;
using CinemaLite.Application.Services.Interfaces.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Ticket.Queries.GetUserTickets;

public class GetUserTicketsQueryHandler(
    IAppDbContext dbContext, 
    ITicketMapper ticketMapper,
    ICurrentUserService currentUserService
) : IRequestHandler<GetUserTicketsQuery, GetUserTicketsResponse>
{
    public async Task<GetUserTicketsResponse> Handle(
        GetUserTicketsQuery request, 
        CancellationToken cancellationToken
    ) {
        var tickets = await dbContext.Tickets
            .Where(t => t.DeletedAt == null && t.UserId == currentUserService.UserId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

        if (tickets == null)
        {
            throw new NotFoundTicketException(currentUserService.UserId);
        }
        
        return ticketMapper.ToGetUserTicketsResponse(tickets);
    }
}