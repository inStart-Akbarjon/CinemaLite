using CinemaLite.Application.CQRS.Movie.Commands.CreateMovie;
using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Interfaces.Mappers;

public interface IMovieMapper
{
    public Movie ToMovieEntityFromCreateMovieCommand(CreateMovieCommand movie);
    public CreateMovieResponse ToCreateMovieResponse(Movie movie);
    public UpdateMovieResponse ToUpdateMovieResponse(Movie movie);
    public GetMovieByIdResponse ToGetMovieByIdResponse(Movie movie);
}