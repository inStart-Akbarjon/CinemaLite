using CinemaLite.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaLite.Infrastructure.Database.Configurations;

public class MovieModelConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");
        
        builder.HasKey(m => m.Id);
        
        // Required Properties
        builder.Property(m => m.Title).IsRequired();
        builder.Property(m => m.DurationMinutes).IsRequired();
        builder.Property(m => m.Genre).IsRequired();
        builder.Property(m => m.Status).IsRequired();
        
        // Relations
        builder
            .Property(m => m.Sessions)
            .HasColumnType("jsonb");
    }
}