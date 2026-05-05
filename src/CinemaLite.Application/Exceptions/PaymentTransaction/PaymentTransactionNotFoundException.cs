using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.PaymentTransaction;

public class PaymentTransactionNotFoundException : AppException
{
    public PaymentTransactionNotFoundException(Guid orderId) : base(
        message: $"The payment transaction with this orderId '{orderId}' not found.",
        statusCode: StatusCodes.Status404NotFound, 
        errorCode: "Payment Transaction Not Found!")
    {
    }
}