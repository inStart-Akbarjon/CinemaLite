using CinemaLite.Domain.Enums;

namespace CinemaLite.Application.DTOs.Order.Response;

public class CreateOrderResponse
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}