using CinemaLite.Application.Exceptions.Cart;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Exceptions.SeatReservation;
using CinemaLite.Application.Exceptions.Session;
using CinemaLite.Application.Exceptions.Ticket;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Cart.Commands.DeleteFromCart;

public class DeleteFromCartCommandHandler(IAppDbContext dbContext) : IRequestHandler<DeleteFromCartCommand, IActionResult>
{
    public async Task<IActionResult> Handle(DeleteFromCartCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var seatReservation = await dbContext.SeatReservations
                .FirstOrDefaultAsync(s => s.CartId == request.CartId && s.Id == request.SeatReservationId, cancellationToken);

            if (seatReservation == null)
            {
                throw new NotFoundSeatReservationException(request.CartId);
            }
            
            var movie = await dbContext.Movies
                .FirstOrDefaultAsync(m => m.Id == seatReservation.MovieId && m.DeletedAt == null, cancellationToken);
            
            if (movie is null)
            {
                throw new NotFoundMovieException(seatReservation.MovieId);
            }
            
            var session = movie.Sessions
                .FirstOrDefault(s => s.Id == seatReservation.SessionId && s.DeletedAt == null);
            
            if (session is null)
            {
                throw new NotFoundSessionException(seatReservation.SessionId);
            }
            
            var seat = session.Seats.FirstOrDefault(s =>
                s.SeatNumber == seatReservation.SeatNumber && s.SeatRow == seatReservation.SeatRow);
            
            if (seat is null)
            {
                throw new NotFoundSeatException(seatReservation.SeatRow, seatReservation.SeatNumber);
            }
            
            seat.Status = SeatStatus.Open;
            
            var cart = await dbContext.Carts
                .FirstOrDefaultAsync(c => c.Id == request.CartId && c.DeletedAt == null, cancellationToken);

            if (cart is null)
            {
                throw new CartNotFoundException(request.CartId);
            }
            
            cart.TotalPrice -= session.Price;
            
            dbContext.Movies.Update(movie);
            dbContext.Carts.Update(cart);
            dbContext.SeatReservations.Remove(seatReservation);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        
        return new OkResult();
    }
}