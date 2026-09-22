using ContactList.Domain.Exceptions;

namespace ContactList.API.Middleware
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                InvalidPhoneNumberException => StatusCodes.Status400BadRequest,
                InvalidEmailException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                success = false,
                message = GetMessage(exception),
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static string GetMessage(Exception exception)
        {
            return exception switch
            {
                ArgumentException => exception.Message,
                KeyNotFoundException => exception.Message,
                UnauthorizedAccessException => exception.Message,
                InvalidPhoneNumberException => exception.Message,
                InvalidEmailException => exception.Message,
                _ => "Something went wrong in server!"
            };
        }
    }
}
