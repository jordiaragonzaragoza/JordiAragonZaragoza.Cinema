namespace JordiAragon.Cinema.Application.Features.Showtime.Events.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Domain.MovieAggregate.Events;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using Microsoft.Extensions.Logging;

    public class MovieShowtimeAddedEventHandler : IDomainEventHandler<ShowtimeAddedEvent>
    {
        private readonly ILogger<MovieShowtimeAddedEventHandler> logger;

        public MovieShowtimeAddedEventHandler(ILogger<MovieShowtimeAddedEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(ShowtimeAddedEvent @event, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Handled Domain Event: {DomainEvent}", @event.GetType().Name);

            ////throw new InvalidOperationException($"{nameof(ToDoItemAddedEventHandler)} has crashed.");
            return Task.CompletedTask;
        }
    }
}