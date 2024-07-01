namespace API;

public class ApiExceptions
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; }

    public ApiExceptions(int statusCode, string message, string? details)
    {
        this.StatusCode = statusCode;
        this.Message = message;
        this.Details = details;
    }
}
