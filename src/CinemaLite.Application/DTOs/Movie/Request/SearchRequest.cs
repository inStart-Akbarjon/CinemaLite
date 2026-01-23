using CinemaLite.Application.DTOs.Pagination;

namespace CinemaLite.Application.DTOs.Movie.Request;

public class SearchRequest
{
    public string SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}