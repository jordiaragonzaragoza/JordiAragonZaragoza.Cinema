namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Domain.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class AuditoriumShowtimeRemovedEventHandler : INotificationHandler<ShowtimeRemovedEvent>
    {
        private readonly ILogger<AuditoriumShowtimeRemovedEventHandler> logger;

        public AuditoriumShowtimeRemovedEventHandler(ILogger<AuditoriumShowtimeRemovedEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(ShowtimeRemovedEvent @event, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Handled Domain Event: {DomainEvent}", @event.GetType().Name);

            ////throw new InvalidOperationException($"{nameof(AuditoriumShowtimeRemovedEventHandler)} has crashed.");
            return Task.CompletedTask;
        }
    }
}