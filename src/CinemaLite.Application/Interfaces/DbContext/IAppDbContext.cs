using CinemaLite.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.Interfaces.DbContext;

public interface IAppDbContext
{
    DbSet<Movie> Movies { get; set; }
    DbSet<Ticket> Tickets { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}