namespace CinemaLite.Application.DTOs.Pagination;

public class PaginatedMovieList<T>
{
    public List<T> Movies { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage { get; set; }
}