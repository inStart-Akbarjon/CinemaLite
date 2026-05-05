using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Application.CQRS.Cart.Commands.DeleteCart;

public record DeleteCartCommand(Guid CartId) : IRequest<IActionResult>
{
    
}