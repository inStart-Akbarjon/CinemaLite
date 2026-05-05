using CinemaLite.Domain.Enums;

namespace CinemaLite.Domain.Models;

public class PaymentTransaction : BaseEntity
{
    public int UserId { get; set; }
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentStatus Status { get; set; }
    public string PaymentIntentId { get; set; }
    public string ClientSecret { get; set; }
    public string UserPhone { get; set; }
    public string UserEmail { get; set; }
}