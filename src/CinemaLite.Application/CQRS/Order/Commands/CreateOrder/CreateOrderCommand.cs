using CinemaLite.Application.DTOs.Order.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Order.Commands.CreateOrder;

public record CreateOrderCommand(Guid CartId) : IRequest<CreateOrderResponse>
{
}