namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies.ExpireReservedSeats
{
    using System;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Application.Helpers;
    using JordiAragonZaragoza.SharedKernel.Contracts;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Interfaces;

    using Microsoft.Extensions.Logging;
    using Quartz;

    // TODO: Replace this batch job to a policy-saga with timeout message.
    // Problems: Polling, non-reactive, difficult to scale..
    // Also coupling with the read model. Not required if using an infrastructure-based process manager (saga-policy).
    [DisallowConcurrentExecution]
    public sealed class ExpireReservedSeatsJob : IJob
    {
        private readonly IDateTime dateTime;
        private readonly ISpecificationReadRepository<ReservationReadModel, Guid> reservationReadModelRepository;
        private readonly ICommandBus commandBus;
        private readonly ILogger<ExpireReservedSeatsJob> logger;
        private readonly IServiceIdentityProvider serviceIdentityProvider;
        private readonly IExecutionContextService executionContextService;

        public ExpireReservedSeatsJob(
            IDateTime dateTime,
            ISpecificationReadRepository<ReservationReadModel, Guid> reservationReadModelRepository,
            ICommandBus commandBus,
            ILogger<ExpireReservedSeatsJob> logger,
            IServiceIdentityProvider serviceIdentityProvider,
            IExecutionContextService executionContextService)
        {
            this.dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
            this.reservationReadModelRepository = reservationReadModelRepository ?? throw new ArgumentNullException(nameof(reservationReadModelRepository));
            this.commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceIdentityProvider = serviceIdentityProvider ?? throw new ArgumentNullException(nameof(serviceIdentityProvider));
            this.executionContextService = executionContextService ?? throw new ArgumentNullException(nameof(executionContextService));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(context, nameof(context));

                var dateTimeUtcNow = this.dateTime.UtcNow;

                var expiredReservations = await this.reservationReadModelRepository.ListAsync(new GetExpiredReservationsSpec(dateTimeUtcNow), context.CancellationToken);
                foreach (var reservation in expiredReservations)
                {
                    // TODO: Complete when using saga-policy.
                    // No need to set the execution context here, since the policy-saga will be infrastructure-based
                    // and will handle the execution context automatically.
                    var executionContext = new ExecutionContext(
                        actorId: ExecutionContext.CreateJobActorId("expire-reserved-seats"),
                        actorType: ActorType.System,
                        executor: this.serviceIdentityProvider.GetName(),
                        executorType: ExecutorType.Worker,
                        correlationId: Guid.NewGuid(),
                        causationId: null,
                        scopeContext: new ScopeContext(SystemConstants.SystemTenantId, null, null));

                    this.executionContextService.SetExecutionContext(executionContext);

                    var result = await this.commandBus.SendAsync(new ExpireReservedSeatsCommand(reservation.ShowtimeId, reservation.Id), context.CancellationToken);
                    if (!result.IsSuccess)
                    {
                        var errorDetails = result.ResultDetails();

                        this.logger.LogError(
                            "Error expiring reservation {ReservationId} for showtime {ShowtimeId}. {@Details}",
                            reservation.Id,
                            reservation.ShowtimeId,
                            errorDetails);
                    }
                }
            }
            #pragma warning disable CA1031
            catch (Exception exception)
            {
                this.logger.LogError(
                   exception,
                   "Error sending: {@Name} Job Command.",
                   nameof(ExpireReservedSeatsCommand));
            }
            #pragma warning restore CA1031
        }
    }
}