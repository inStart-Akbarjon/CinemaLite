using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Order;

public class OrderCancellationException : AppException
{
    public OrderCancellationException() : base(
        message: $"The order can be cancelled only if the status is Pending",
        statusCode: StatusCodes.Status404NotFound, 
        errorCode: "Order Can Not Be Cancelled!")
    {
    }
}