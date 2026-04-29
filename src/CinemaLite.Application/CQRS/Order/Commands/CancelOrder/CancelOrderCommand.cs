using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Application.CQRS.Order.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : IRequest<IActionResult>
{
}