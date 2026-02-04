namespace CinemaLite.Application.Models.Cache;

public static class MoviesCacheKeys
{
    public static string Page(int pageNumber, int pageSize)
    {
        return $"movies:pages:{pageNumber}:{pageSize}";
    }

    public const string Registry = "movies:registry";
}