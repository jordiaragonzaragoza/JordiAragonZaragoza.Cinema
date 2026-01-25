namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies.ShowtimeEnded
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;

    public sealed class EndShowtimeInAuditoriumPolicy : BaseEventHandler<ShowtimeEndedEvent>
    {
        private readonly ICommandBus commandBus;

        public EndShowtimeInAuditoriumPolicy(
            ICommandBus commandBus)
        {
            this.commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
        }

        public override async Task HandleAsync(ShowtimeEndedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            await this.commandBus.SendAsync(new EndShowtimeInAuditoriumCommand(@event.AuditoriumId, @event.AggregateId), cancellationToken);
        }
    }
}