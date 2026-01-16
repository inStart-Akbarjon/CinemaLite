namespace CinemaLite.Application.DTOs.Ticket.Response;

public class CreateTicketResponse
{
    public Guid TicketId { get; set; }
    public string MovieTitle { get; set; }
    public string CinemaName { get; set; }
    public DateTime StartTime { get; set; }
    public decimal PricePaid { get; set; }
}