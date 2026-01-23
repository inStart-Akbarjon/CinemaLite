namespace CinemaLite.Application.DTOs.Ticket.Response;

public class GetUserTicketsResponse
{
    public List<GetUserTickets> Tickets { get; set; }
}

public class GetUserTickets
{
    public required Guid TicketId { get; set; }
    public required string MovieTitle { get; set; }
    public required string CinemaName { get; set; }
    public int SeatRow { get; set; }
    public int SeatNumber { get; set; }
    public required DateTime StartTime { get; set; }
    public required decimal PricePaid { get; set; }
}