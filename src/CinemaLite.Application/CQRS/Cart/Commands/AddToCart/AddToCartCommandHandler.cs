using CinemaLite.Application.DTOs.Cart.Response;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Exceptions.Session;
using CinemaLite.Application.Exceptions.Ticket;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Services.Implementations.RedisDistributedLock;
using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Contracts.Events;
using CinemaLite.Domain.Enums;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Cart.Commands.AddToCart;

public class AddTicketToCartCommandHandler(
    IAppDbContext dbContext,
    ISeatReservationMapper seatReservationMapper,
    ICurrentUserService currentUserService,
    IDistributedLockService distributedLockService,
    ISendEndpointProvider sendEndpointProvider) : IRequestHandler<AddToCartCommand, AddSeatReservationToCartResponse>
{
    public async Task<AddSeatReservationToCartResponse> Handle(
        AddToCartCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = currentUserService.UserId;

        var cart = await dbContext.Carts.FirstOrDefaultAsync(c => c.CustomerId == userId && c.DeletedAt == null, cancellationToken);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var endpoint = await sendEndpointProvider.GetSendEndpoint(
            new Uri("queue:reservation.expire.delay.queue"));
        
        try
        {
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

            var seat = session.Seats.FirstOrDefault(s=>
                s.Id == request.SeatId);

            if (seat is null)
            {
                throw new NotFoundSeatException(request.SeatId);
            }

            var lockKey = $"lock:seat:{request.SessionId}:{seat.SeatRow}:{seat.SeatNumber}";

            using var seatLock = await distributedLockService.AcquireAsync(
                lockKey,
                TimeSpan.FromSeconds(5),
                cancellationToken);

            if (seatLock is null)
            {
                throw new ConcurrentBookingException(seat.SeatRow, seat.SeatNumber);
            }

            if (seat.Status == SeatStatus.Booked)
            {
                throw new BookedSeatException(seat.SeatRow, seat.SeatNumber);
            }
            
            if (seat.Status == SeatStatus.Reserved)
            {
                throw new ReservedSeatException(seat.SeatRow, seat.SeatNumber);
            }

            seat.Status = SeatStatus.Reserved;
            
            Guid cartId;
            
            if (cart is null)
            {
                cartId = Guid.NewGuid();
                await dbContext.Carts.AddAsync(new Domain.Models.Cart()
                {
                    Id = cartId,
                    CustomerId = userId,
                    TotalPrice = session.Price,
                }, cancellationToken);

                await endpoint.Send(new CartCreatedEvent()
                {
                    CartId = cartId
                }, cancellationToken);
            }
            else
            {
                cartId = cart.Id;
                cart.TotalPrice += session.Price;
            }
            
            var seatReservation = seatReservationMapper.ToSeatReservationEntity(
                request,
                movie,
                session,
                seat,
                cartId);

            session.ReduceAvailableSeatsByOne(session.AvailableSeats);
            dbContext.Movies.Update(movie);
            await dbContext.SeatReservations.AddAsync(seatReservation, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return seatReservationMapper.ToCreateSeatReservationResponse(seatReservation);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}