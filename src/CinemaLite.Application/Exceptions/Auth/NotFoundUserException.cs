using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Auth;

public class NotFoundUserException: AppException
{
    public NotFoundUserException(string email) : base(
        message: $"The user with this email '{email}' not found.",
        statusCode: StatusCodes.Status404NotFound, 
        errorCode: "Not found user")
    {
    }
}