using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Mappers;

public class SessionMapper : ISessionMapper
{
    public CreateSessionResponse ToCreateSessionResponse(Session session)
    {

        return new CreateSessionResponse()
        {
            Id = session.Id,
            CinemaName = session.CinemaName,
            AvailableSeats = session.AvailableSeats,
            Price = session.Price,
            StartTime = session.StartTime
        };
    }

    public UpdateSessionResponse ToUpdateSessionResponse(Session session)
    {
        return new UpdateSessionResponse()
        {
            Id = session.Id,
            CinemaName = session.CinemaName,
            Price = session.Price,
            AvailableSeats = session.AvailableSeats,
            StartTime = session.StartTime
        };
    }

    public GetSessionByIdResponse ToGetSessionByIdResponse(Session session)
    {
        return new GetSessionByIdResponse()
        {
            Id = session.Id,
            CinemaName = session.CinemaName,
            AvailableSeats = session.AvailableSeats,
            Price = session.Price,
            StartTime = session.StartTime
        };
    }
}