namespace JordiAragonZaragoza.Cinema.Reservation.Worker.ReadModelMigrator
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class MigratorBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private readonly ILogger<MigratorBackgroundWorker> logger;
        private readonly ActivitySource activitySource;

        public MigratorBackgroundWorker(
            IServiceProvider serviceProvider,
            IHostEnvironment hostEnvironment,
            IHostApplicationLifetime hostApplicationLifetime,
            ILogger<MigratorBackgroundWorker> logger)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            this.activitySource = new ActivitySource(this.hostEnvironment.ApplicationName);
            this.hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void Dispose()
        {
            this.activitySource?.Dispose();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var activity = this.activitySource.StartActivity(this.hostEnvironment.ApplicationName, ActivityKind.Client);

            try
            {
                using var scope = this.serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ReservationReadModelContext>();

                this.logger.LogInformation("Starting migration of the read model");

                await RunMigrationAsync(dbContext, stoppingToken);

                this.logger.LogInformation("Migration completed successfully");
            }
            catch (Exception exception)
            {
                activity?.AddException(exception);
                throw;
            }

            this.hostApplicationLifetime.StopApplication();
        }

        private static async Task RunMigrationAsync(ReservationReadModelContext dbContext, CancellationToken cancellationToken)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
            });

            /*await strategy.ExecuteAsync(
                state: 0,
                operation: async (context, state, ct) =>
                {
                    await context.Database.MigrateAsync(ct);
                    return 0;
                },
                verifySucceeded: async (context, state, ct) =>
                {
                    var canConnect = await context.Database.CanConnectAsync(ct);
                    return new ExecutionResult<int>(canConnect, canConnect ? 0 : -1);
                },
                cancellationToken: cancellationToken);
                }*/
        }
    }
}