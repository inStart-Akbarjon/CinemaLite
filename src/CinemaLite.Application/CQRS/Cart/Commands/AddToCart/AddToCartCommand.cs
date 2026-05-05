using CinemaLite.Application.DTOs.Cart.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Cart.Commands.AddToCart;

public record AddToCartCommand(
    Guid MovieId,
    Guid SessionId,
    int SeatRow,
    int SeatNumber
) : IRequest<AddSeatReservationToCartResponse>
{
}