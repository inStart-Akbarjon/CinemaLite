using CinemaLite.Domain.Enums;

namespace CinemaLite.Domain.Models;

public class AuditLog
{
    public Guid Id { get; set; }
    public string TableName { get; set; }
    public Guid EntityId { get; set; }
    public AuditLogAction Action { get; set; }
    public string ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; }
    public List<AuditLogDetails> AuditLogDetails { get; set; } = [];
}