namespace Blog.BL.Exception_Handling;

public class BusinessException : Exception
{
    public int StatusCode { get; }
    public string? Details { get; }
    public BusinessException(int statusCode, string message) : base(message) => StatusCode = statusCode;

    public BusinessException(int statusCode, string message, string details) : base(message)
    {
        StatusCode = statusCode;
        Details = details;
    }
}
