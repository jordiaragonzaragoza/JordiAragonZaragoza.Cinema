namespace JordiAragon.Cinemas.Ticketing.Application.Showtime.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class AuditoriumShowtimeAddedEventHandler : INotificationHandler<ShowtimeAddedEvent>
    {
        private readonly ILogger<AuditoriumShowtimeAddedEventHandler> logger;

        public AuditoriumShowtimeAddedEventHandler(ILogger<AuditoriumShowtimeAddedEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(ShowtimeAddedEvent @event, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Handled Domain Event: {DomainEvent}", @event.GetType().Name);

            ////throw new InvalidOperationException($"{nameof(AuditoriumShowtimeAddedEventHandler)} has crashed.");
            return Task.CompletedTask;
        }
    }
}