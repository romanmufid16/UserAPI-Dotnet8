namespace UserAPI_Dotnet8.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                var statusCode = ex switch
                {
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    BadHttpRequestException badReq => badReq.StatusCode, // misal dari validasi manual
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    ApplicationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                var title = statusCode switch
                {
                    400 => "Bad Request",
                    401 => "Unauthorized",
                    404 => "Not Found",
                    500 => "Internal Server Error",
                    _ => "Error"
                };

                var response = new
                {
                    title,
                    status = statusCode,
                    errors = ex.Message
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
