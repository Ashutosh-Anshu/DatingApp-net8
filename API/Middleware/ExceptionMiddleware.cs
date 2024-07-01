using System.Net;
using System.Text.Json;

namespace API;

public class ExceptionMiddleware
{
    public RequestDelegate next { get; set; }
    public ILogger<ExceptionMiddleware> logger { get; set; }
    public IHostEnvironment env { get; set; }

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        this.next = next;
        this.logger = logger;
        this.env = env;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = env.IsDevelopment() ?
                new ApiExceptions(context.Response.StatusCode, ex.Message, ex.StackTrace) :
                new ApiExceptions(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

}
