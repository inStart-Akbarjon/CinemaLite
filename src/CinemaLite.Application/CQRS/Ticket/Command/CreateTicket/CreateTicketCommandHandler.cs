using System.Security.Claims;
using CinemaLite.Application.DTOs.Ticket.Response;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Exceptions.Session;
using CinemaLite.Application.Exceptions.Ticket;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Services.Implementations.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Ticket.Command.CreateTicket;

public class CreateTicketCommandHandler(
    IAppDbContext dbContext, 
    ITicketMapper ticketMapper,
    ICurrentUserService currentUserService) : IRequestHandler<CreateTicketCommand, CreateTicketResponse>
{
    public async Task<CreateTicketResponse> Handle(
        CreateTicketCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = currentUserService.UserId;
        
        var movie = await dbContext.Movies
            .FirstOrDefaultAsync(m => m.Id == request.MovieId && m.DeletedAt == null, cancellationToken);

        if (movie is null)
        {
            throw new NotFoundMovieException(request.MovieId);
        }
        
        var session = movie.Sessions
            .FirstOrDefault(s => s.Id == request.SessionId && s.DeletedAt == null);

        if (session is null)
        {
            throw new NotFoundSessionException(request.SessionId);
        }

        if (session.AvailableSeats < 1)
        {
            throw new NoAvailableSeatsException(session.Id);
        }
        
        var ticket = ticketMapper.ToTicketEntity(request, movie, session, userId);
        
        session.ReduceAvailableSeatsByOne(session.AvailableSeats);
        dbContext.Movies.Update(movie);
        await dbContext.Tickets.AddAsync(ticket, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);   
        
        return  ticketMapper.ToCreateTicketResponse(ticket);
    }
}