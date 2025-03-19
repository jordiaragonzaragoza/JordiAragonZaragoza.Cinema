namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.BackgroundJobs.ExpireReservedSeats
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Application.Helpers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using Microsoft.Extensions.Logging;
    using Quartz;

    // TODO: Replace this batch job to a policy-saga with timeout message.
    [DisallowConcurrentExecution]
    public sealed class ExpireReservedSeatsJob : IJob
    {
        private readonly IDateTime dateTime;
        private readonly ISpecificationReadRepository<ReservationReadModel, Guid> reservationReadModelRepository;
        private readonly ICommandBus commandBus;
        private readonly ILogger<ExpireReservedSeatsJob> logger;

        public ExpireReservedSeatsJob(
            IDateTime dateTime,
            ISpecificationReadRepository<ReservationReadModel, Guid> reservationReadModelRepository,
            ICommandBus commandBus,
            ILogger<ExpireReservedSeatsJob> logger)
        {
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
            this.reservationReadModelRepository = Guard.Against.Null(reservationReadModelRepository, nameof(reservationReadModelRepository));
            this.commandBus = Guard.Against.Null(commandBus, nameof(commandBus));
            this.logger = Guard.Against.Null(logger, nameof(logger));
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
                    var result = await this.commandBus.SendAsync(new ExpireReservedSeatsCommand(reservation.ShowtimeId, reservation.Id), context.CancellationToken);
                    if (!result.IsSuccess)
                    {
                        var errorDetails = result.ResultDetails();

                        this.logger.LogError(
                               "Error sending: {@Name} Job Command. {@Details}",
                               nameof(ExpireReservedSeatsCommand),
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