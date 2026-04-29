using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Order;

public class OrderNotFoundException : AppException
{
    public OrderNotFoundException(Guid orderId) : base(
        message: $"The order with this orderId '{orderId}' not found.",
        statusCode: StatusCodes.Status404NotFound, 
        errorCode: "Order Not Found!")
    {
    }
}