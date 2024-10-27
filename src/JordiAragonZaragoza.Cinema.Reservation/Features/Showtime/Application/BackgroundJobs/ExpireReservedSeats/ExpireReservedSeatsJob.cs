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
        private readonly ISpecificationReadRepository<TicketReadModel, Guid> ticketReadModelRepository;
        private readonly ICommandBus commandBus;
        private readonly ILogger<ExpireReservedSeatsJob> logger;

        public ExpireReservedSeatsJob(
            IDateTime dateTime,
            ISpecificationReadRepository<TicketReadModel, Guid> ticketReadModelRepository,
            ICommandBus commandBus,
            ILogger<ExpireReservedSeatsJob> logger)
        {
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
            this.ticketReadModelRepository = Guard.Against.Null(ticketReadModelRepository, nameof(ticketReadModelRepository));
            this.commandBus = Guard.Against.Null(commandBus, nameof(commandBus));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Guard.Against.Null(context, nameof(context));

                var dateTimeUtcNow = this.dateTime.UtcNow;

                var expiredTickets = await this.ticketReadModelRepository.ListAsync(new GetExpiredTicketsSpec(dateTimeUtcNow), context.CancellationToken);
                foreach (var ticket in expiredTickets)
                {
                    var result = await this.commandBus.SendAsync(new ExpireReservedSeatsCommand(ticket.ShowtimeId, ticket.Id), context.CancellationToken);
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