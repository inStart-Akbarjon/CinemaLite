using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Auth;

public class DuplicateUserEmailException : AppException
{
    public DuplicateUserEmailException(string email) : base(
        message: $"User with this email '{email}' already exists.", 
        statusCode: StatusCodes.Status400BadRequest, 
        errorCode: "Duplicating User Email")
    {
    }
}