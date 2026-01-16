using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Movie;

public class DuplicateMovieException : AppException
{
    public DuplicateMovieException(string title) : base(
        message: $"Movie with this Title '{title}' already exists.", 
        statusCode: StatusCodes.Status400BadRequest, 
        errorCode: "Duplicating movie Title")
    {
    }
}