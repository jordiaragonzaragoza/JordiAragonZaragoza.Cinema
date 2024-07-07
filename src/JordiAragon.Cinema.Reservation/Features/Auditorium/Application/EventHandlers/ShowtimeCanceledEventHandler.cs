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

    public sealed class ShowtimeCanceledEventHandler : IEventHandler<ShowtimeCanceledEvent>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public ShowtimeCanceledEventHandler(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public async Task Handle(ShowtimeCanceledEvent @event, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(@event.AuditoriumId), cancellationToken)
                                    ?? throw new NotFoundException(nameof(Auditorium), @event.AuditoriumId.ToString());

            existingAuditorium.RemoveShowtime(ShowtimeId.Create(@event.AggregateId));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);
        }
    }
}