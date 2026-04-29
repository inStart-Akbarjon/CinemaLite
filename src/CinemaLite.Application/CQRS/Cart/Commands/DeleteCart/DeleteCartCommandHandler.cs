using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Services.Interfaces.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Cart.Commands.DeleteCart;

public class DeleteCartCommandHandler(IAppDbContext dbContext, IOpenSeatsStatus openSeatsStatus) : IRequestHandler<DeleteCartCommand, IActionResult>
{
    public async Task<IActionResult> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var cart = await dbContext.Carts.FirstOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);
            
            await openSeatsStatus.OpenSeatsStatusFromCart(request.CartId, cancellationToken);

            if (cart != null)
            {
                dbContext.Carts.Remove(cart);
            }
            
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            return new OkResult();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}