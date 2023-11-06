namespace JordiAragon.Cinemas.Reservation.Auditorium.Application.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain.Specifications;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class ShowtimeCreatedEventHandler : INotificationHandler<ShowtimeCreatedEvent>
    {
        private readonly IRepository<Auditorium> auditoriumRepository;

        public ShowtimeCreatedEventHandler(
            IRepository<Auditorium> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public async Task Handle(ShowtimeCreatedEvent @event, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(new AuditoriumByIdSpec(AuditoriumId.Create(@event.AuditoriumId)), cancellationToken)
                                     ?? throw new NotFoundException(nameof(Auditorium), @event.AuditoriumId.ToString());

            existingAuditorium.AddShowtime(ShowtimeId.Create(@event.ShowtimeId));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);
        }
    }
}