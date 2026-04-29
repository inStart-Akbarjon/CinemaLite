using CinemaLite.Application.DTOs.Cart.Response;
using CinemaLite.Application.Exceptions.Cart;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Services.Interfaces.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Cart.Queries.GetUserCart;

public class GetUserCartQueryHandler(
    IAppDbContext dbContext, 
    ICurrentUserService currentUserService,
    ICartMapper cartMapper) : IRequestHandler<GetUserCartQuery, GetUserCartResponse>
{
    public async Task<GetUserCartResponse> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        
        var cart = await dbContext.Carts.FirstOrDefaultAsync(c => c.CustomerId == userId, cancellationToken);

        if (cart is null)
        {
            throw new CartNotFoundException(userId);
        }
        
        return cartMapper.ToGetUserCartResponse(cart);
    }
}