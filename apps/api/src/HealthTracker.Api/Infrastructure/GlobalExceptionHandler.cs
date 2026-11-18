using Microsoft.AspNetCore.Diagnostics;

namespace HealthTracker.Api.Infrastructure;

/// <summary>
/// Turns any unhandled exception into an RFC 9457 ProblemDetails response without leaking internals.
/// Bad requests (e.g. an unparsable route value) keep their 4xx status and are logged as warnings.
/// </summary>
internal sealed partial class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var method = httpContext.Request.Method;
        var path = httpContext.Request.Path.Value ?? string.Empty;

        int status;
        string title;
        string? detail = null;

        if (exception is BadHttpRequestException badRequest)
        {
            status = badRequest.StatusCode;
            title = "The request is invalid.";
            detail = badRequest.Message;
            LogBadRequest(logger, method, path, badRequest.Message);
        }
        else
        {
            status = StatusCodes.Status500InternalServerError;
            title = "An unexpected error occurred.";
            LogUnhandled(logger, exception, method, path);
        }

        httpContext.Response.StatusCode = status;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = { Status = status, Title = title, Detail = detail },
        });
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Unhandled exception for {Method} {Path}")]
    private static partial void LogUnhandled(ILogger logger, Exception exception, string method, string path);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Bad request for {Method} {Path}: {Reason}")]
    private static partial void LogBadRequest(ILogger logger, string method, string path, string reason);
}
