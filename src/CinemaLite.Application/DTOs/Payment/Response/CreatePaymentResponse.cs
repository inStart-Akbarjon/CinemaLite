using CinemaLite.Domain.Enums;

namespace CinemaLite.Application.DTOs.Payment.Response;

public class CreatePaymentResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentStatus Status { get; set; }
    public string ClientSecret { get; set; }
    public string PaymentIntentId { get; set; }
}