using CinemaLite.Application.DTOs.Session.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Session.Commands.CreateSession;

public record CreateSessionCommand(
    Guid MovieId,
    string CinemaName,
    int AvailableSeats,
    decimal Price,
    DateTime StartTime
) : IRequest<CreateSessionResponse>
{
}