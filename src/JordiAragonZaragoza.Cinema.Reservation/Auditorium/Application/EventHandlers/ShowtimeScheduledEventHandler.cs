namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.EventHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeScheduledEventHandler : BaseEventHandler<ShowtimeScheduledEvent>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public ShowtimeScheduledEventHandler(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = auditoriumRepository ?? throw new ArgumentNullException(nameof(auditoriumRepository));
        }

        public override async Task HandleAsync(ShowtimeScheduledEvent notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(new AuditoriumId(notification.AuditoriumId), cancellationToken)
                                     ?? throw new NotFoundException(nameof(Auditorium), notification.AuditoriumId.ToString());

            existingAuditorium.AddActiveShowtime(new ShowtimeId(notification.AggregateId));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);
        }
    }
}