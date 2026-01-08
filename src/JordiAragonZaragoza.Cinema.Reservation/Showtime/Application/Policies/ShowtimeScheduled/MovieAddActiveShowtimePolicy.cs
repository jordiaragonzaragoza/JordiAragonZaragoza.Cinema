namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies.ShowtimeScheduled
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;

    public sealed class MovieAddActiveShowtimePolicy : BaseEventHandler<ShowtimeScheduledEvent>
    {
        private readonly ICommandBus commandBus;

        public MovieAddActiveShowtimePolicy(
            ICommandBus commandBus)
        {
            this.commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
        }

        public override async Task HandleAsync(ShowtimeScheduledEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            await this.commandBus.SendAsync(new AddActiveShowtimeCommand(@event.MovieId, @event.AggregateId), cancellationToken);
        }
    }
}