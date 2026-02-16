using CinemaLite.Application.CQRS.Movie.Commands.CreateMovie;
using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Enums;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Mappers;

public class MovieMapper : IMovieMapper
{
    public Movie ToMovieEntityFromCreateMovieCommand(CreateMovieCommand movie)
    {
        return new Movie
        {
            Title = movie.Title,
            DurationMinutes = movie.DurationMinutes,
            Genre = movie.Genre,
            IsTop = movie.IsTop,
            TopSubscriptionPeriod = movie.TopSubscriptionPeriod,
            Status = MovieStatus.UnPublished
        };
    }

    public CreateMovieResponse ToCreateMovieResponse(Movie movie)
    {
        var movieStatus = Enum.GetName(typeof(MovieStatus), movie.Status);
        
        return new CreateMovieResponse()
        {
            Id = movie.Id,
            Title = movie.Title,
            DurationMinutes = movie.DurationMinutes,
            Genre = movie.Genre,
            IsTop = movie.IsTop,
            TopSubscriptionPeriod = movie.TopSubscriptionPeriod,
            Status = movieStatus
        };
    }
    
    public UpdateMovieResponse ToUpdateMovieResponse(Movie movie)
    {
        var movieStatus = Enum.GetName(typeof(MovieStatus), movie.Status);
        
        return new UpdateMovieResponse()
        {
            Id = movie.Id,
            Title = movie.Title,
            DurationMinutes = movie.DurationMinutes,
            Genre = movie.Genre,
            MinPrice = movie.MinPrice,
            IsTop = movie.IsTop,
            TopSubscriptionPeriod = movie.TopSubscriptionPeriod,
            Status = movieStatus
        };
    }
    
    public GetMovieByIdResponse ToGetMovieByIdResponse(Movie movie)
    {
        var movieStatus = Enum.GetName(typeof(MovieStatus), movie.Status);
        
        return new GetMovieByIdResponse()
        {
            Id = movie.Id,
            Title = movie.Title,
            DurationMinutes = movie.DurationMinutes,
            MinPrice = movie.MinPrice,
            Genre = movie.Genre,
            IsTop = movie.IsTop,
            TopSubscriptionPeriod = movie.TopSubscriptionPeriod,
            Status = movieStatus
        };
    }
}