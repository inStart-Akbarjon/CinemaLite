using CinemaLite.Application.DTOs.Session.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Session.Queries.GetSessionById;

public record GetSessionByIdQuery(Guid Id, Guid MovieId) : IRequest<GetSessionByIdResponse>
{
}