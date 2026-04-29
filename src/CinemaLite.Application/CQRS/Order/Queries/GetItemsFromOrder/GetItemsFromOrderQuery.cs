using CinemaLite.Application.DTOs.Order.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Order.Queries.GetItemsFromOrder;

public record GetItemsFromOrderQuery(Guid OrderId) : IRequest<GetItemsFromOrderResponse>
{
    
}