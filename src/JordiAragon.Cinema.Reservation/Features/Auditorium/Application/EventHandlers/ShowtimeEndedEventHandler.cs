namespace JordiAragon.Cinema.Reservation.Auditorium.Application.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeEndedEventHandler : IEventHandler<ShowtimeEndedEvent>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public ShowtimeEndedEventHandler(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public async Task Handle(ShowtimeEndedEvent notification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(notification, nameof(notification));

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(notification.AuditoriumId), cancellationToken)
                                    ?? throw new NotFoundException(nameof(Auditorium), notification.AuditoriumId.ToString());

            existingAuditorium.RemoveActiveShowtime(ShowtimeId.Create(notification.AggregateId));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);
        }
    }
}