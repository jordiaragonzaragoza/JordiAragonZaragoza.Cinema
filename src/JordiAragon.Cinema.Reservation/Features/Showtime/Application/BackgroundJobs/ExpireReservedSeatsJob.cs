namespace JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Quartz;

    [DisallowConcurrentExecution]
    public class ExpireReservedSeatsJob : IJob
    {
        private readonly IDateTime dateTime;
        private readonly ISpecificationReadRepository<Showtime, ShowtimeId> showtimeReadRepository;
        private readonly ISender sender;
        private readonly ILogger<ExpireReservedSeatsJob> logger;

        public ExpireReservedSeatsJob(
            IDateTime dateTime,
            ISpecificationReadRepository<Showtime, ShowtimeId> showtimeReadRepository,
            ISender sender,
            ILogger<ExpireReservedSeatsJob> logger)
        {
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
            this.showtimeReadRepository = Guard.Against.Null(showtimeReadRepository, nameof(showtimeReadRepository));
            this.sender = Guard.Against.Null(sender, nameof(sender));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var dateTimeUtcNow = this.dateTime.UtcNow;
                var showtimesWithExpiredTickets = await this.showtimeReadRepository.ListAsync(new GetExpiredReserveSeatsSpec(dateTimeUtcNow), context.CancellationToken);
                foreach (var showtime in showtimesWithExpiredTickets)
                {
                    var ticketIds = showtime.Tickets.Where(ticket => !ticket.IsPurchased && dateTimeUtcNow > ticket.CreatedTimeOnUtc.AddMinutes(1)).Select(t => t.Id).ToList();

                    foreach (var ticketId in ticketIds)
                    {
                        var result = await this.sender.Send(new ExpireReservedSeatsCommand(showtime.Id.Value, ticketId.Value), context.CancellationToken);
                        if (!result.IsSuccess)
                        {
                            // TODO: Complete log the error from result.
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                   exception,
                   "Error sending: {@Name} Job Command.",
                   nameof(ExpireReservedSeatsCommand));
            }
        }
    }
}