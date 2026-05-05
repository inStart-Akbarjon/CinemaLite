using CinemaLite.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaLite.Infrastructure.Database.Configurations;

public class CartModelConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Cart");
        
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(c => new {c.CustomerId});
    }
}