namespace JordiAragon.Cinema.Application.Features.Auditorium.Ticket.BackgroundJobs
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Ticket.Commands;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Quartz;

    [DisallowConcurrentExecution]
    public class ExpireReservedSeatsJob : IJob
    {
        private readonly IDateTime dateTime;
        private readonly IReadRepository<Ticket> ticketRepository;
        private readonly ISender sender;
        private readonly ILogger<ExpireReservedSeatsJob> logger;

        public ExpireReservedSeatsJob(
            IDateTime dateTime,
            IReadRepository<Ticket> ticketRepository,
            ISender sender,
            ILogger<ExpireReservedSeatsJob> logger)
        {
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
            this.ticketRepository = Guard.Against.Null(ticketRepository, nameof(ticketRepository));
            this.sender = Guard.Against.Null(sender, nameof(sender));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var expiredTickets = await this.ticketRepository.ListAsync(new GetExpiredReserveSeatsSpec(this.dateTime.UtcNow), context.CancellationToken);
                foreach (var ticket in expiredTickets)
                {
                    var result = await this.sender.Send(new ExpireReservedSeatsCommand(ticket.Id.Value), context.CancellationToken);
                    if (!result.IsSuccess)
                    {
                        // TODO: Complete log the error from result.
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