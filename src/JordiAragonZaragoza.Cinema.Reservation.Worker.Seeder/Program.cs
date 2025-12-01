namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Program class should not have a protected constructor or the static keyword because is used in WebApplicationFactory for functional and integration test.")]
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            ConfigurationManager configuration = builder.Configuration;

            builder.AddInfrastructure();

            // Configure specific Host Services (DI)
            builder.Services
                .AddHostedService<BackgroundWorker>()
                .AddInfrastructureEntityFrameworkMigrations(configuration)
                .AddHostConfigurations(configuration);

            builder.AddInfrastructureEntityFrameworkMigrations();

            IHost app = builder.Build();

            ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogDebug("Read Model Migrator Host created...");

            await app.RunAsync();
        }
    }
}