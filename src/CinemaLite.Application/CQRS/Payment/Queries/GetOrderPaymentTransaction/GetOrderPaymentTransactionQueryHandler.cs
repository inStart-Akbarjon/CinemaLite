using CinemaLite.Application.DTOs.Payment.Response;
using CinemaLite.Application.Exceptions.PaymentTransaction;
using CinemaLite.Application.Interfaces.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Payment.Queries.GetOrderPaymentTransaction;

public class GetOrderPaymentTransactionQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetOrderPaymentTransactionQuery, GetOrderPaymentTransactionResponse>
{
    public async Task<GetOrderPaymentTransactionResponse> Handle(GetOrderPaymentTransactionQuery request, CancellationToken cancellationToken)
    {
        var transaction = await dbContext.PaymentTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.OrderId == request.OrderId, cancellationToken);

        if (transaction == null)
        {
            throw new PaymentTransactionNotFoundException(request.OrderId);
        }
        
        return new GetOrderPaymentTransactionResponse()
        {
            TotalAmount = transaction.TotalAmount,
            Status = transaction.Status,
            PaymentIntentId = transaction.PaymentIntentId,
            ClientSecret = transaction.ClientSecret,
        };
    }
}