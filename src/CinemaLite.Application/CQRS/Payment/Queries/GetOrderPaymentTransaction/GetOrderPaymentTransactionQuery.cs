using CinemaLite.Application.DTOs.Payment.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Payment.Queries.GetOrderPaymentTransaction;

public record GetOrderPaymentTransactionQuery(Guid OrderId) : IRequest<GetOrderPaymentTransactionResponse>
{
}