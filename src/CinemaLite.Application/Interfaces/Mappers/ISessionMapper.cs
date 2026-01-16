using CinemaLite.Application.CQRS.Session.Commands.CreateSession;
using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Interfaces.Mappers;

public interface ISessionMapper
{
    public CreateSessionResponse ToCreateSessionResponse(Session session);
    
    public UpdateSessionResponse ToUpdateSessionResponse(Session session);
    
    public GetSessionByIdResponse ToGetSessionByIdResponse(Session session);
}