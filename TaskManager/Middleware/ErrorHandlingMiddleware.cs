using System.Net;
using System.Text.Json;
using TaskManager.Exceptions;

namespace TaskManager.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Um erro inesperado ocorreu durante o processamento da requisição.");

            var response = context.Response;
            response.ContentType = "application/json";

            var (statusCode, message) = GetStatusCodeAndMessage(exception);
            response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new { error = message });
            await response.WriteAsync(result);
        }

        private (HttpStatusCode statusCode, string message) GetStatusCodeAndMessage(Exception exception)
        {
            return exception switch
            {
                ProjectNotFoundException or TaskNotFoundException or UserNotFoundException
                    => (HttpStatusCode.NotFound, exception.Message),

                UnauthorizedOperationException
                    => (HttpStatusCode.Forbidden, exception.Message),

                TaskLimitExceededException or ProjectDeletionException
                    => (HttpStatusCode.BadRequest, exception.Message),

                UnauthorizedAccessException
                    => (HttpStatusCode.Unauthorized, exception.Message),

                _ => (HttpStatusCode.InternalServerError, "Erro inesperado. Tente novamente mais tarde.")
            };
        }
    }
}