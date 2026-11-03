namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Forwards the caller's identity and execution context to the Reservation APIs.
    /// Single place to replace the token pass-through by a token exchange (Keycloak token exchange,
    /// Entra On-Behalf-Of) if the MCP gateway ever needs its own audience.
    /// </summary>
    public sealed class AuthorizationDelegatingHandler : DelegatingHandler
    {
        // Allow list: x-causation-id is deliberately not forwarded, clients must not set it.
        private static readonly string[] ForwardedContextHeaders =
        [
            "x-tenant-id",
            "x-partition-id",
            "x-domain-id",
            "x-correlation-id",
        ];

        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var inbound = this.httpContextAccessor.HttpContext?.Request.Headers;
            if (inbound is not null)
            {
                if (AuthenticationHeaderValue.TryParse(inbound.Authorization.ToString(), out var authorization))
                {
                    request.Headers.Authorization = authorization;
                }

                foreach (var headerName in ForwardedContextHeaders)
                {
                    if (inbound.TryGetValue(headerName, out var values) && values.Count > 0)
                    {
                        request.Headers.TryAddWithoutValidation(headerName, values.ToArray());
                    }
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}