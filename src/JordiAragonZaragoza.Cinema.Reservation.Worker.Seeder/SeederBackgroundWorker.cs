namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using ExecutionContext = JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces.ExecutionContext;

    public sealed class SeederBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private readonly ILogger<SeederBackgroundWorker> logger;
        private readonly ActivitySource activitySource;

        public SeederBackgroundWorker(
            IServiceProvider serviceProvider,
            IHostEnvironment hostEnvironment,
            IHostApplicationLifetime hostApplicationLifetime,
            ILogger<SeederBackgroundWorker> logger)
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
            using var scope = this.serviceProvider.CreateScope();
            var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            var executionContextService = scope.ServiceProvider.GetRequiredService<IExecutionContextService>();
            executionContextService.SetExecutionContext(CreateSeederExecutionContext());

            try
            {
                this.logger.LogInformation("Starting seeding data on business model");

                await SeedData.PopulateBusinessModelTestDataAsync(eventStore, stoppingToken);

                this.logger.LogInformation("Data seeding completed successfully");
            }
            catch (Exception exception)
            {
                activity?.AddException(exception);
                throw;
            }

            this.hostApplicationLifetime.StopApplication();
        }

        private static ExecutionContext CreateSeederExecutionContext()
        {
            var testContext = new ExecutionContext(
                actorId: ExecutionContext.CreateServiceActorId("reservation-worker-seeder"), // TODO: Review.
                actorType: ActorType.System,
                executor: nameof(SeederBackgroundWorker),
                executorType: ExecutorType.Worker,
                correlationId: Guid.CreateVersion7(),
                causationId: null,
                scopeContext: new ScopeContext(
                    tenantId: SystemConstants.SystemTenantId,
                    partitionId: null,
                    domainId: null));

            return testContext;
        }
    }
}