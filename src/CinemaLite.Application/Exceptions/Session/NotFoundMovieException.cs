using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Session;

public class NotFoundSessionException : AppException
{
    public NotFoundSessionException(Guid id) : base(
        message: $"The session with this Id '{id}' not found.",
        statusCode: StatusCodes.Status404NotFound, 
        errorCode: "Session Not Found!")
    {
    }
}