namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ModelContextProtocol;
    using ModelContextProtocol.Protocol;
    using ModelContextProtocol.Server;

    public static class ExceptionHandlingFilters
    {
        public static McpRequestFilter<CallToolRequestParams, CallToolResult> CreateGlobalToolExceptionHandler()
        {
            return next => async (request, cancellationToken) =>
            {
                try
                {
                    return await next(request, cancellationToken);
                }
                catch (HttpRequestException exception)
                {
                    throw new McpException($"HTTP request failed: {exception.Message}", exception);
                }
                catch (HttpIOException exception)
                {
                    throw new McpException($"HTTP I/O error: {exception.Message}", exception);
                }
                catch (TaskCanceledException exception)
                {
                    throw new McpException($"Request timeout: {exception.Message}", exception);
                }
                catch (Exception exception) when (exception is not McpException)
                {
                    throw new McpException($"Unexpected error in tool execution: {exception.GetType().Name}", exception);
                }
            };
        }
    }
}