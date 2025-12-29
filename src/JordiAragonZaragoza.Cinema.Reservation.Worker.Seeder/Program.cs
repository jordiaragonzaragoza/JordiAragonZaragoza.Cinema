namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Program class should not have a protected constructor or the static keyword because is used in WebApplicationFactory for functional and integration test.")]
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            var configuration = builder.Configuration;

            builder.AddInfrastructure();
            builder.AddInfrastructureKurrentDbClient();

            // Configure specific Host Services (DI)
            builder.Services
                .AddHostedService<BackgroundWorker>()
                .AddInfrastructureEventStoreSeeder()
                .AddHostConfigurations(configuration);

            IHost app = builder.Build();

            ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogDebug("Reservation Seeder Host created...");

            await app.RunAsync();
        }
    }
}