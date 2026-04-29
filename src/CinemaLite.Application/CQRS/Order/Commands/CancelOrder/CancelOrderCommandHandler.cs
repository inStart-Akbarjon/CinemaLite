using CinemaLite.Application.Exceptions.Order;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Services.Interfaces.Common;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Order.Commands.CancelOrder;

public class CancelOrderCommandHandler(
    IAppDbContext dbContext, 
    IOpenSeatsStatus openSeatsStatus) : IRequestHandler<CancelOrderCommand, IActionResult>
{
    public async Task<IActionResult> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        if (order.Status == OrderStatus.Pending)
        {
            order.Status = OrderStatus.Canceled;
        }
        else
        {
            throw new OrderCancellationException();
        }
        
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        await openSeatsStatus.OpenSeatsStatusFromOrder(request.OrderId, cancellationToken);
        
        return new OkResult();
    }
}