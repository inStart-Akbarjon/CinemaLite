using CinemaLite.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaLite.Infrastructure.Database.Configurations;

public class AuditLogModelConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLog");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.AuditLogDetails)
            .HasColumnType("jsonb");
    }
}