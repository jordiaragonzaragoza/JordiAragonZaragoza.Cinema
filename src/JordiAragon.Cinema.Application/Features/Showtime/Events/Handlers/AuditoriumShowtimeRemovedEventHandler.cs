namespace JordiAragon.Cinema.Application.Features.Showtime.Events.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Events;
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