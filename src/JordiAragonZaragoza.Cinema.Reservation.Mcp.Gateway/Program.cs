namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Logging;
    using JordiAragonZaragoza.Cinema.ServiceDefaults;
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Hosting.Configuration;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common.Configuration;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common.Infrastructure;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Major Code Smell",
        "S1118:Utility classes should not have public constructors",
        Justification = "Used by WebApplicationFactory for functional and integration tests.")]
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.AddServiceDefaults();

            // Authentication: validates the user's JWT and publishes the protected resource metadata
            // (RFC 9728) so MCP clients can discover the authorization server.
            builder.Services.AddMcpGatewayAuthentication(configuration, builder.Environment);

            // Configure specific Host Services (DI)
            builder.Services.AddMcpServer()

                    // Stateless: every request carries its own bearer token and no in-memory session is kept,
                    // so the gateway can run with several replicas without session affinity.
                    .WithHttpTransport(options => options.Stateless = true)
                    .WithToolsFromAssembly(McpGatewayAssemblyReference.Assembly)
                    .WithResourcesFromAssembly(McpGatewayAssemblyReference.Assembly)
                    .WithPromptsFromAssembly(McpGatewayAssemblyReference.Assembly)
                    .WithRequestFilters(filters =>
                    {
                        filters.AddCallToolFilter(ExceptionHandlingFilters.CreateGlobalToolExceptionHandler());
                    });

            builder.Services
                .AddInfrastructure();

            builder.AddHostConfigurations();
            builder.WebHost.UseWebHostBuilderConfigurations();

            var app = builder.Build();

            app.Logger.LogDebug("Reservation Mcp Gateway Host created...");

            // Configure Request Pipeline
            ConfigureWebApplication.UseWebApplicationConfigurations(app)
                                   .MapDefaultEndpoints();

            app.Run();
        }
    }
}