using CinemaLite.Application.Services.Implementations.RedisDistributedLock;
using CinemaLite.Application.Services.Implementations.Auth;
using CinemaLite.Application.DTOs.Ticket.Response;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Exceptions.Session;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Exceptions.Ticket;
using CinemaLite.Application.Exceptions.Movie;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CinemaLite.Application.CQRS.Ticket.Command.CreateTicket;

public class CreateTicketCommandHandler(
    IAppDbContext dbContext, 
    ITicketMapper ticketMapper,
    ICurrentUserService currentUserService,
    IDistributedLockService distributedLockService) : IRequestHandler<CreateTicketCommand, CreateTicketResponse>
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

        var seat = session.Seats.FirstOrDefault(s => s.SeatNumber == request.SeatNumber && s.SeatRow == request.SeatRow);

        if (seat is null)
        {
            throw new NotFoundSeatException(request.SeatRow, request.SeatNumber);
        }

        var lockKey = $"lock:seat:{request.SessionId}:{request.SeatRow}:{request.SeatNumber}";
        
        using var seatLock = await distributedLockService.AcquireAsync(
            lockKey,
            TimeSpan.FromSeconds(5),
            cancellationToken);

        if (seatLock is null)
        {
            throw new ConcurrentBookingException(request.SeatRow, request.SeatNumber);
        }
        
        if (seat.IsBooked)
        {
            throw new ReservedSeatException(request.SeatRow, request.SeatNumber);
        }
        
        seat.IsBooked = true;
        
        var ticket = ticketMapper.ToTicketEntity(request, movie, session, seat, userId);
        
        session.ReduceAvailableSeatsByOne(session.AvailableSeats);
        dbContext.Movies.Update(movie);
        await dbContext.Tickets.AddAsync(ticket, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);   
        
        return  ticketMapper.ToCreateTicketResponse(ticket);
    }
}