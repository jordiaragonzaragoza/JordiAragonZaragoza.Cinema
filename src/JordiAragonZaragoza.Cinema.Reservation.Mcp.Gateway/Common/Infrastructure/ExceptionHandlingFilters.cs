namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common.Infrastructure
{
    using System;
    using System.Net;
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
                    when (exception.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                {
                    throw new McpException(
                        $"The Reservation API denied the request ({(int)exception.StatusCode}). " +
                        "The user may lack permissions for the tenant, partition or domain sent in the headers.",
                        exception);
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
