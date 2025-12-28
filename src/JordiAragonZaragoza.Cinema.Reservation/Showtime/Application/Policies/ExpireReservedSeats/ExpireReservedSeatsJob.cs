namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies.ExpireReservedSeats
{
    using System;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Application.Helpers;
    using JordiAragonZaragoza.SharedKernel.Contracts;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
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
            this.dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
            this.reservationReadModelRepository = reservationReadModelRepository ?? throw new ArgumentNullException(nameof(reservationReadModelRepository));
            this.commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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