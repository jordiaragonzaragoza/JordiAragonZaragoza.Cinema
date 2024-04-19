namespace JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs.EndShowtimes
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.SharedKernel.Application.Helpers;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Quartz;

    // TODO: Replace this batch job to a policy-saga with timeout message.
    [DisallowConcurrentExecution]
    public sealed class EndShowtimesJob : IJob
    {
        private readonly IDateTime dateTime;
        private readonly ISpecificationReadRepository<Showtime, ShowtimeId> showtimeReadRepository;
        private readonly IShowtimeManager showtimeManager;
        private readonly ISender internalBus;
        private readonly ILogger<EndShowtimesJob> logger;

        public EndShowtimesJob(
            IDateTime dateTime,
            ISpecificationReadRepository<Showtime, ShowtimeId> showtimeReadRepository,
            IShowtimeManager showtimeManager,
            ISender internalBus,
            ILogger<EndShowtimesJob> logger)
        {
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
            this.showtimeReadRepository = Guard.Against.Null(showtimeReadRepository, nameof(showtimeReadRepository));
            this.showtimeManager = Guard.Against.Null(showtimeManager, nameof(showtimeManager));
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var dateTimeUtcNow = this.dateTime.UtcNow;

                var currentAndPreviousDayShowtimes = await this.showtimeReadRepository.ListAsync(new CurrentAndPreviousDayNotEndedShowtimesSpec(dateTimeUtcNow), context.CancellationToken);
                foreach (var showtime in currentAndPreviousDayShowtimes)
                {
                    var isEnded = await this.showtimeManager.HasShowtimeEndedAsync(showtime, dateTimeUtcNow, context.CancellationToken);
                    if (isEnded)
                    {
                        await this.SendEndShowtimeCommandAsync(showtime, context.CancellationToken);
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                   exception,
                   "Error sending: {@Name} Job Command.",
                   nameof(EndShowtimesJob));
            }
        }

        private async Task SendEndShowtimeCommandAsync(Showtime showtime, CancellationToken cancellationToken)
        {
            var result = await this.internalBus.Send(new EndShowtimeCommand(showtime.Id), cancellationToken);
            if (!result.IsSuccess)
            {
                var errorDetails = result.ResultDetails();

                this.logger.LogError(
                       "Error sending: {@Name} Job Command. {@Details}",
                       nameof(EndShowtimesJob),
                       errorDetails);
            }
        }
    }
}