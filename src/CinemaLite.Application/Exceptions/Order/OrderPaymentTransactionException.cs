using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Order;

public class OrderPaymentTransactionException : AppException
{
    public OrderPaymentTransactionException(Guid orderId) : base(
        message: $"The order with this orderId '{orderId}' cannot have more than one payment transaction.",
        statusCode: StatusCodes.Status404NotFound,
        errorCode: "Possibility of more than one PaymentTransaction!")
    {
    }
}