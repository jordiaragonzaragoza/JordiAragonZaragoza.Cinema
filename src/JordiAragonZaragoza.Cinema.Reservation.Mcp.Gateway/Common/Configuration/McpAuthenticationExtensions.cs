namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common.Configuration
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Configuration;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using ModelContextProtocol.AspNetCore.Authentication;
    using ModelContextProtocol.Authentication;

    public static class McpAuthenticationExtensions
    {
        private const string AuthorityKey = "Authentication:Schemes:Bearer:Authority";
        private const string ResourceUrlKey = "Mcp:ResourceUrl";

        public static IServiceCollection AddMcpGatewayAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(environment);

            var authority = configuration[AuthorityKey];
            var resourceUrl = configuration[ResourceUrlKey];

            if (string.IsNullOrWhiteSpace(authority) || string.IsNullOrWhiteSpace(resourceUrl))
            {
                throw new InvalidOperationException(
                    $"'{AuthorityKey}' and '{ResourceUrlKey}' must be configured for the MCP gateway.");
            }

            // The SDK metadata takes plain strings, so validate here that they are absolute URLs.
            if (!Uri.TryCreate(authority, UriKind.Absolute, out _)
                || !Uri.TryCreate(resourceUrl, UriKind.Absolute, out _))
            {
                throw new InvalidOperationException(
                    $"'{AuthorityKey}' and '{ResourceUrlKey}' must be absolute URLs.");
            }

            // Same provider agnostic JWT validation used by the APIs (Authority, Audience, claims).
            services.AddSharedKernelJwtBearerAuthentication(configuration, environment);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = McpAuthenticationDefaults.AuthenticationScheme;
            })
            .AddMcp(options =>
            {
                // Served at /.well-known/oauth-protected-resource. It tells MCP clients which
                // authorization server issues the tokens this gateway accepts.
                options.ResourceMetadata = new ProtectedResourceMetadata
                {
                    Resource = resourceUrl,
                    AuthorizationServers = { authority },
                    ScopesSupported = ["openid", "profile"],
                };
            });

            services.AddAuthorization();

            return services;
        }
    }
}
