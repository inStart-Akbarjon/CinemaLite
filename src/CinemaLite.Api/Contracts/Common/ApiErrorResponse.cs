namespace CinemaLite.Api.Contracts.Common;

public record ApiErrorResponse
{
    public string? Title { get; set; }
    public int Status { get; set; }
    public string? Details { get; set; }
}