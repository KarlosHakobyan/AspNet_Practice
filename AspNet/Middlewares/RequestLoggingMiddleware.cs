
using System.Diagnostics;

namespace AspNet.Api.Middlewares;

public sealed class RequestLoggingMiddleware : IMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context,RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        var request = context.Request;

        var method = request.Method;
        var path = request.Path;
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = request.Headers.UserAgent.ToString();
        var traceId = context.TraceIdentifier;

        _logger.LogInformation(
            """
            HTTP Request:
            Method: {Method}
            Path: {Path}
            IP Address: {IPAddress}
            User-Agent: {UserAgent}
            TraceId: {TraceId}
            """,
            method,
            path,
            ipAddress,
            userAgent,
            traceId
        );

        try
        {
            await next(context);
        }
        finally 
        {
            stopwatch.Stop();

            _logger.LogInformation(
                """
                HTTP Response:
                Method: {Method}
                Path: {Path}
                Status Code: {StatusCode}
                Duration: {ElapsedMilliseconds} ms
                TraceId: {TraceId}
                """,
                method,
                path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                traceId
            );
        }
    }
}
