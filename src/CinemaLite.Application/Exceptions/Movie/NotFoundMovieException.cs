using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Movie;

public class NotFoundMovieException : AppException
{
    public NotFoundMovieException(Guid id) : base(
        message: $"The movie with this Id '{id}' not found.",
        statusCode: StatusCodes.Status404NotFound, 
        errorCode: "Movie Not Found!")
    {
    }
}