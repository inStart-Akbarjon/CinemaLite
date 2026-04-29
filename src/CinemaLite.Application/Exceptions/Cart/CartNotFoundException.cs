using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Cart;

public class CartNotFoundException : AppException
{
    public CartNotFoundException(int userId) : base(
        message: $"The cart with this user Id '{userId}' not found.",
        statusCode: StatusCodes.Status404NotFound, 
        errorCode: "Cart Not Found!")
    {
    }
    
    public CartNotFoundException(Guid cartId) : base(
        message: $"The cart with this cart Id '{cartId}' not found.",
        statusCode: StatusCodes.Status404NotFound, 
        errorCode: "Cart Not Found!")
    {
    }
}