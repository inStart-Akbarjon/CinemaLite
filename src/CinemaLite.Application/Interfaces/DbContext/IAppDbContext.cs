using CinemaLite.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CinemaLite.Application.Interfaces.DbContext;

public interface IAppDbContext
{
    DbSet<Movie> Movies { get; set; }
    DbSet<Ticket> Tickets { get; set; }
    DbSet<Cart> Carts { get; set; }
    DbSet<Order> Orders { get; set; }
    DbSet<SeatReservation> SeatReservations { get; set; }
    DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    
    public DatabaseFacade Database
    {
        get;
    }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}