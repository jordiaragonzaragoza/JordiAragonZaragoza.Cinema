namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeScheduledEventHandler : IEventHandler<ShowtimeScheduledEvent>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public ShowtimeScheduledEventHandler(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public async Task Handle(ShowtimeScheduledEvent notification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(notification, nameof(notification));

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(new AuditoriumId(notification.AuditoriumId), cancellationToken)
                                     ?? throw new NotFoundException(nameof(Auditorium), notification.AuditoriumId.ToString());

            existingAuditorium.AddActiveShowtime(new ShowtimeId(notification.AggregateId));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);
        }
    }
}