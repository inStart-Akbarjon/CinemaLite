using CinemaLite.Application.DTOs.Movie.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Movie.Commands.CreateMovie;

public record CreateMovieCommand(
    string Title,
    int DurationMinutes,
    string Genre,
    bool IsTop,
    int TopSubscriptionPeriod
) : IRequest<CreateMovieResponse>
{
}