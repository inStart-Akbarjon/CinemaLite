using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Auth;

public class InValidPasswordException : AppException
{
    public InValidPasswordException(string email) : base(
        message: $"The password provided by user with this email '{email}' is not correct.", 
        statusCode: StatusCodes.Status400BadRequest, 
        errorCode: "Invalid Password")
    {
    }
}