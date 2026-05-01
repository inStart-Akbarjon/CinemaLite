using CinemaLite.Application.DTOs.Cart.Response;
using CinemaLite.Application.Exceptions.Ticket;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Cart.Queries.GetItemsFromCart;

public class GetItemsFromCartQueryHandler(
    IAppDbContext dbContext, 
    ICartMapper cartMapper) : IRequestHandler<GetItemsFromCartQuery, GetItemsFromCartResponse>
{
    public async Task<GetItemsFromCartResponse> Handle(GetItemsFromCartQuery request, CancellationToken cancellationToken)
    {
        var seatReservations = await dbContext.SeatReservations
            .Where(s => s.CartId == request.CartId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (seatReservations is null)
        {
            throw new NotFoundTicketException(request.CartId);
        }
        
        return cartMapper.ToGetItemsFromCartResponse(seatReservations);
    }
}