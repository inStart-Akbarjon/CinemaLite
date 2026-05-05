using CinemaLite.Application.DTOs.Cart.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Cart.Queries.GetItemsFromCart;

public record GetItemsFromCartQuery(Guid CartId) : IRequest<GetItemsFromCartResponse>
{
    
}