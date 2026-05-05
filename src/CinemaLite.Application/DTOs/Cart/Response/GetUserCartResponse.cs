namespace CinemaLite.Application.DTOs.Cart.Response;

public class GetUserCartResponse
{
    public Guid Id { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
}