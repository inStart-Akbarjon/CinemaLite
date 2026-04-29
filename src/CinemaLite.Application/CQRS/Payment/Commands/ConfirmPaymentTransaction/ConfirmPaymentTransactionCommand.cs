using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Application.CQRS.Payment.Commands.ConfirmPaymentTransaction;

public record ConfirmPaymentTransactionCommand(string PaymentIntentId, PaymentStatus Status) : IRequest<IActionResult>
{
}