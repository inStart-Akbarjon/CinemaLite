using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Application.CQRS.Cart.Commands.DeleteFromCart;

public record DeleteFromCartCommand(Guid CartId, Guid SeatReservationId) : IRequest<IActionResult>
{
    
}