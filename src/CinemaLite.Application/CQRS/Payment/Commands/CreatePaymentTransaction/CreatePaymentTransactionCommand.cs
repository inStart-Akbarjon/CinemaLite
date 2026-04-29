using CinemaLite.Application.DTOs.Payment.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Payment.Commands.CreatePaymentTransaction;

public record CreatePaymentTransactionCommand(Guid OrderId) : IRequest<CreatePaymentResponse>
{
}