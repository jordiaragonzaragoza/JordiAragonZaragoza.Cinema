namespace JordiAragon.Cinemas.Ticketing.Application.Showtime.BackgroundJobs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Commands;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Quartz;

    [DisallowConcurrentExecution]
    public class ExpireReservedSeatsJob : IJob
    {
        private readonly IDateTime dateTime;
        private readonly IReadRepository<Showtime> showtimeRepository;
        private readonly ISender sender;
        private readonly ILogger<ExpireReservedSeatsJob> logger;

        public ExpireReservedSeatsJob(
            IDateTime dateTime,
            IReadRepository<Showtime> showtimeRepository,
            ISender sender,
            ILogger<ExpireReservedSeatsJob> logger)
        {
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.sender = Guard.Against.Null(sender, nameof(sender));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var dateTimeUtcNow = this.dateTime.UtcNow;
                var showtimesWithExpiredTickets = await this.showtimeRepository.ListAsync(new GetExpiredReserveSeatsSpec(dateTimeUtcNow), context.CancellationToken);
                foreach (var showtime in showtimesWithExpiredTickets)
                {
                    var ticketIds = showtime.Tickets.Where(ticket => !ticket.IsPaid && dateTimeUtcNow > ticket.CreatedTimeOnUtc.AddMinutes(1)).Select(t => t.Id).ToList();

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