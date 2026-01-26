using CinemaLite.Application.DTOs.Session.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Session.Commands.UpdateSession;

public record UpdateSessionCommand(
    Guid Id, 
    Guid MovieId, 
    string CinemaName, 
    decimal Price, 
    int TotalRows,
    int SeatsPerRow,
    DateTime StartTime
) : IRequest<UpdateSessionResponse>
{
}