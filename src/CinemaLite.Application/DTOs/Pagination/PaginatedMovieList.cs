namespace CinemaLite.Application.DTOs.Pagination;

public class PaginatedQueryList<T>
{
    public List<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage { get; set; }
}