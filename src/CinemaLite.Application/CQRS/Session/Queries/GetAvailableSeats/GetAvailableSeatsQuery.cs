using CinemaLite.Application.DTOs.Session.Respone;
using MediatR;

namespace CinemaLite.Application.CQRS.Session.Queries.GetAvailableSeats;

public record GetAvailableSeatsQuery(Guid Id, Guid MovieId) : IRequest<GetAvailableSeatsResponse>
{
}