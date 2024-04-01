namespace JordiAragon.Cinema.Reservation.Showtime.Application.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public sealed class AuditoriumShowtimeAddedEventHandler : INotificationHandler<ShowtimeAddedEvent>
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