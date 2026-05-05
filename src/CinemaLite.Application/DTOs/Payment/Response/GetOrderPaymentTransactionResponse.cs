using CinemaLite.Domain.Enums;

namespace CinemaLite.Application.DTOs.Payment.Response;

public class GetOrderPaymentTransactionResponse
{
    public decimal TotalAmount { get; set; }
    public PaymentStatus Status { get; set; }
    public string PaymentIntentId { get; set; }
    public string ClientSecret { get; set; }
}