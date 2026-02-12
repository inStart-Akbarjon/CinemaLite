namespace CinemaLite.Application.Models.Cache;

public static class TopMoviesCacheKeys
{
    public static string Page(int pageNumber, int pageSize)
    {
        return $"topMovies:pages:{pageNumber}:{pageSize}";
    }

    public const string Registry = "topMovies:registry";
}