using Microsoft.AspNetCore.Identity;

namespace CinemaLite.Domain.Models;

public class ApplicationUser : IdentityUser<int>
{
    public ICollection<Ticket> Tickets { get; set; }
}