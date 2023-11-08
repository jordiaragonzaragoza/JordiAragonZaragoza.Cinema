namespace JordiAragon.Cinema.Reservation.Showtime.Application.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Reservation.Movie.Domain.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class MovieShowtimeAddedEventHandler : INotificationHandler<ShowtimeAddedEvent>
    {
        private readonly ILogger<MovieShowtimeAddedEventHandler> logger;

        public MovieShowtimeAddedEventHandler(ILogger<MovieShowtimeAddedEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(ShowtimeAddedEvent @event, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Handled Domain Event: {DomainEvent}", @event.GetType().Name);

            ////throw new InvalidOperationException($"{nameof(MovieShowtimeAddedEventHandler)} has crashed.");
            return Task.CompletedTask;
        }
    }
}