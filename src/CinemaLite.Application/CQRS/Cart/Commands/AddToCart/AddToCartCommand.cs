using CinemaLite.Application.DTOs.Cart.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Cart.Commands.AddToCart;

public record AddToCartCommand(
    Guid MovieId,
    Guid SessionId,
    Guid SeatId
) : IRequest<AddSeatReservationToCartResponse>
{
}