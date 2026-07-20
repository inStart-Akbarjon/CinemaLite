using System.Security.Claims;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Domain.Enums;
using CinemaLite.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CinemaLite.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor)
    : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options), IAppDbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<SeatReservation> SeatReservations { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modifiedEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        var auditLogs = new List<AuditLog>();

        foreach (var modifiedEntity in modifiedEntities)
        {
            var userEmail = contextAccessor
                .HttpContext?
                .User
                .FindFirstValue(ClaimTypes.Email) ?? "System";

            var entityId = modifiedEntity
                .Property("Id")
                .CurrentValue?
                .ToString() ?? "";

            if (modifiedEntity.State == EntityState.Added)
            {
                var auditLog = new AuditLog()
                {
                    Id = Guid.NewGuid(),
                    TableName = modifiedEntity.Entity.GetType().Name,
                    EntityId = entityId,
                    Action = AuditLogAction.Created,
                    ChangedAt = DateTime.UtcNow,
                    ChangedBy = userEmail
                };

                auditLogs.Add(auditLog);
            }
            else if (modifiedEntity.State == EntityState.Modified &&
                     modifiedEntity.Property("DeletedAt").CurrentValue == null)
            {
                var auditLogChanges = GetChanges(modifiedEntity);

                if (auditLogChanges != null)
                {
                    var auditLog = new AuditLog()
                    {
                        Id = Guid.NewGuid(),
                        TableName = modifiedEntity.Entity.GetType().Name,
                        EntityId = entityId,
                        Action = AuditLogAction.Updated,
                        ChangedAt = DateTime.UtcNow,
                        ChangedBy = userEmail,
                        AuditLogDetails = auditLogChanges,
                    };

                    auditLogs.Add(auditLog);
                }
            }
            else if (modifiedEntity.Property("DeletedAt").CurrentValue != null
                     || modifiedEntity.State == EntityState.Deleted)
            {
                var auditLog = new AuditLog()
                {
                    Id = Guid.NewGuid(),
                    TableName = modifiedEntity.Entity.GetType().Name,
                    EntityId = entityId,
                    Action = AuditLogAction.Deleted,
                    ChangedAt = DateTime.UtcNow,
                    ChangedBy = userEmail
                };

                auditLogs.Add(auditLog);
            }
        }

        if (auditLogs.Count > 0)
        {
            AuditLogs.AddRange(auditLogs);
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    private List<AuditLogDetails>? GetChanges(EntityEntry modifiedEntity)
    {
        var changes = new List<AuditLogDetails>();

        foreach (var property in modifiedEntity.OriginalValues.Properties)
        {
            var originalValue = modifiedEntity.OriginalValues[property];
            var currentValue = modifiedEntity.CurrentValues[property];

            if (!Equals(originalValue, currentValue))
            {
                changes.Add(new AuditLogDetails()
                {
                    PropertyName = property.Name,
                    OldValue = originalValue?.ToString() ?? "Null",
                    NewValue = currentValue?.ToString() ?? "Null",
                });
            }
        }

        if (changes.Count == 0)
        {
            return null;
        }

        return changes;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}