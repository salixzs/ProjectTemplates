using Salix.AspNetCore.JsonExceptionHandler;

namespace WebApiTemplate.WebApi.Middleware;

public class ApiJsonErrorMiddleware : ApiJsonExceptionMiddleware
{
    // use either this simplified constructor
    public ApiJsonErrorMiddleware(RequestDelegate next, ILogger<ApiJsonExceptionMiddleware> logger, bool showStackTrace)
        : base(next, logger, showStackTrace)
    {
    }

    // or use this constructor to supply extended options
    public ApiJsonErrorMiddleware(RequestDelegate next, ILogger<ApiJsonExceptionMiddleware> logger, ApiJsonExceptionOptions options)
        : base(next, logger, options)
    {
    }
}
