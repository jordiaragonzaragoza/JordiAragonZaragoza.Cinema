namespace JordiAragon.Cinema.Application.Features.Showtime.Events.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Events;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using Microsoft.Extensions.Logging;

    public class AuditoriumShowtimeAddedEventHandler : IDomainEventHandler<ShowtimeAddedEvent>
    {
        private readonly ILogger<AuditoriumShowtimeAddedEventHandler> logger;

        public AuditoriumShowtimeAddedEventHandler(ILogger<AuditoriumShowtimeAddedEventHandler> logger)
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