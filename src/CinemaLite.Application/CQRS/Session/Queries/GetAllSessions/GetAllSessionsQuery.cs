using CinemaLite.Application.DTOs.Session.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Session.Queries.GetAllSessions;

public record GetAllSessionsQuery(Guid MovieId) : IRequest<GetAllSessionsFromMovieResponse>
{
        
};