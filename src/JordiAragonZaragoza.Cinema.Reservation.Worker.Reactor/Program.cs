namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Reactor
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Reactor.Configuration;
    using JordiAragonZaragoza.SharedKernel.Application;
    using JordiAragonZaragoza.SharedKernel.Domain;
    using JordiAragonZaragoza.SharedKernel.Infrastructure;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Major Code Smell",
        "S1118:Utility classes should not have public constructors",
        Justification = "Used by WebApplicationFactory for functional and integration tests.")]
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            var configuration = builder.Configuration;

            builder.AddInfrastructure();
            builder.AddInfrastructureEventStoreDbClient();

            // Configure specific Host Services (DI)
            builder.Services
                .AddDomain()
                .AddApplicationValidators()
                .AddApplicationCommandHandlers()
                .AddInfrastructureEventStoreDbRepositories();

            // Then configure SharedKernel Services (DI)
            builder.Services
                .AddSharedKernelDomain()
                .AddSharedKernelApplicationCommandBus()
                .AddSharedKernelInfrastructureEventStoreDbBusiness(configuration)
                .AddSharedKernelInfrastructure()
                .AddSharedKernelInfrastructureCommandBus();

            builder.Services.AddHostConfigurations(configuration);

            IHost app = builder.Build();

            ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogDebug("Reservation Worker Reactor Host created...");

            await app.RunAsync();
        }
    }
}