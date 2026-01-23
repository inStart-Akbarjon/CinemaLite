using CinemaLite.Application.DTOs.Session.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Session.Commands.CreateSession;

public record CreateSessionCommand(
    Guid MovieId,
    string CinemaName,
    decimal Price,
    int TotalRows,
    int SeatsPerRow,
    DateTime StartTime
) : IRequest<CreateSessionResponse>
{
}