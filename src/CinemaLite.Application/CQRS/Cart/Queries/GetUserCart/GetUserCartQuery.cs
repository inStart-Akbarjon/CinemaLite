using CinemaLite.Application.DTOs.Cart.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Cart.Queries.GetUserCart;

public record GetUserCartQuery() : IRequest<GetUserCartResponse>
{

}