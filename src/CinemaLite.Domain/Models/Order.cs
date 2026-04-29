using CinemaLite.Domain.Enums;

namespace CinemaLite.Domain.Models;

public class Order : BaseEntity
{
    public int OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}