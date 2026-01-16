using CinemaLite.Application.DTOs.Session.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Session.Commands.CreateSession;

public record CreateSessionCommand(
    DateTime StartTime,
    string CinemaName,
    int AvailableSeats,
    decimal Price,
    Guid MovieId
) : IRequest<CreateSessionResponse>
{
}