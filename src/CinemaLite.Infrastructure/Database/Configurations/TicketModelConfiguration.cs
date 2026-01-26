using CinemaLite.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaLite.Infrastructure.Database.Configurations;

public class TicketModelConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");
        
        builder.HasKey(t => t.Id);
        
        // Required Properties
        builder.Property(t => t.UserId).IsRequired();
        builder.Property(t => t.MovieId).IsRequired();
        builder.Property(t => t.SessionId).IsRequired();
        builder.Property(t => t.MovieTitle).IsRequired();
        builder.Property(t => t.CinemaName).IsRequired();
        builder.Property(t => t.SeatRow).IsRequired();
        builder.Property(t => t.SeatNumber).IsRequired();
        builder.Property(t => t.StartTime).IsRequired();
        builder.Property(t => t.PricePaid).IsRequired();
    }
}