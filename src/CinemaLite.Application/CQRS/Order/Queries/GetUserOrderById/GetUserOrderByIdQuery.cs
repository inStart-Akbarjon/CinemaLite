using CinemaLite.Application.DTOs.Order.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Order.Queries.GetUserOrderById;

public record GetUserOrderByIdQuery(Guid OrderId) : IRequest<GetUserOrderByIdResponse>
{
}