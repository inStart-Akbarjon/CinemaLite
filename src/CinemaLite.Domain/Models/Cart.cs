namespace CinemaLite.Domain.Models;

public class Cart : BaseEntity
{
    public int CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}