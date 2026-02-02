using CinemaLite.Application.DTOs.Session.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Session.Queries.GetSessionSeats;

public record GetSessionSeatsQuery(Guid Id, Guid MovieId) : IRequest<GetAvailableSeatsResponse>
{
}