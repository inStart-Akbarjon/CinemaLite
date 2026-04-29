using CinemaLite.Application.DTOs.Order.Response;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Order.Queries.GetItemsFromOrder;

public class GetItemsFromOrderQueryHandler(
    IAppDbContext dbContext, 
    IOrderMapper orderMapper) : IRequestHandler<GetItemsFromOrderQuery, GetItemsFromOrderResponse>
{
    public async Task<GetItemsFromOrderResponse> Handle(GetItemsFromOrderQuery request, CancellationToken cancellationToken)
    {
        var seatReservations = await dbContext.SeatReservations.Where(o => o.OrderId == request.OrderId).ToListAsync(cancellationToken);
        
        return orderMapper.ToGetItemsFromOrderResponse(seatReservations);
    }
}