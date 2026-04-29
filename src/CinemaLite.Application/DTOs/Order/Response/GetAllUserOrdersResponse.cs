using CinemaLite.Domain.Enums;

namespace CinemaLite.Application.DTOs.Order.Response;

public class GetAllUserOrdersResponse
{
    public Guid Id { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}