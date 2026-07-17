using CinemaLite.Application.DTOs.Ticket.Response;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Exceptions.Session;
using CinemaLite.Application.Exceptions.Ticket;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Ticket.Commands.RefundTicket;

public class RefundTicketCommandHandler(IAppDbContext dbContext)
    : IRequestHandler<RefundTicketCommand, RefundTicketResponse>
{
    public async Task<RefundTicketResponse> Handle(RefundTicketCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var ticket = await dbContext.Tickets
                .FirstOrDefaultAsync(t => t.Id == request.TicketId && t.DeletedAt == null, cancellationToken);

            if (ticket == null)
            {
                throw new NotFoundTicketException(request.TicketId);
            }

            if (ticket.StartTime <= DateTime.UtcNow)
            {
                throw new ExpiredTicketException(request.TicketId);
            }
            
            var movie = await dbContext.Movies
                .FirstOrDefaultAsync(
                    m => m.Id == ticket.MovieId && m.DeletedAt == null && m.Status == MovieStatus.Published,
                    cancellationToken);

            if (movie == null)
            {
                throw new NotFoundMovieException(ticket.MovieId);
            }

            var session = movie.Sessions.FirstOrDefault(s => s.Id == ticket.SessionId && s.DeletedAt == null);

            if (session == null)
            {
                throw new NotFoundSessionException(ticket.SessionId);
            }

            var seat = session.Seats.FirstOrDefault(s =>
                s.SeatRow == ticket.SeatRow && s.SeatNumber == ticket.SeatNumber);

            if (seat == null)
            {
                throw new NotFoundSeatException(ticket.SeatRow, ticket.SeatNumber);
            }

            seat.Status = SeatStatus.Open;

            var today = DateTime.UtcNow.Date;
            var movieDate = session.StartTime.Date;
            decimal refundAmount = 0;
            
            if (movieDate == today)
            {
                refundAmount = session.Price * 0.5m;
            }
            else
            {
                refundAmount = session.Price * 0.7m;
            }

            ticket.SoftDelete();
            
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new RefundTicketResponse
            {
                ReturnedPrice = refundAmount
            };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}