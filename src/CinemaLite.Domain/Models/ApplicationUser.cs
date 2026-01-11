using Microsoft.AspNetCore.Identity;

namespace CinemaLite.Domain.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string PasswordHash { get; set; }
    public int Coins { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}