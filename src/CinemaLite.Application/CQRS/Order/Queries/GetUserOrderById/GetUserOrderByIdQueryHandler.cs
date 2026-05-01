using CinemaLite.Application.DTOs.Order.Response;
using CinemaLite.Application.Exceptions.Order;
using CinemaLite.Application.Interfaces.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Order.Queries.GetUserOrderById;

public class GetUserOrderByIdQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetUserOrderByIdQuery, GetUserOrderByIdResponse>
{
    public async Task<GetUserOrderByIdResponse> Handle(GetUserOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        return new GetUserOrderByIdResponse()
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            TotalPrice = order.TotalPrice,
            Status = order.Status
        };
    }
}