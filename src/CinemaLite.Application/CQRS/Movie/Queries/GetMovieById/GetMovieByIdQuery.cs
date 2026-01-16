using CinemaLite.Application.DTOs.Movie.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetMovieById;

public record GetMovieByIdQuery (Guid Id) : IRequest<GetMovieByIdResponse>
{
    
}