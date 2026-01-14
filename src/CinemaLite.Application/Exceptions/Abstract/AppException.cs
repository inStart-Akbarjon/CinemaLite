namespace CinemaLite.Application.Exceptions.Abstract;

public abstract class AppException : Exception
{
    public int StatusCode { get; set; }
    public string ErrorCode { get; set; }

    protected AppException(
        string? message, 
        int statusCode, 
        string errorCode
    ) : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}