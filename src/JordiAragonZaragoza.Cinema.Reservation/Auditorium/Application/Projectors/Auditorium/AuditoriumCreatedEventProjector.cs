namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Projectors.Auditorium
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class AuditoriumCreatedEventProjector : BaseEventHandler<AuditoriumCreatedEvent>
    {
        private readonly IRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository;

        public AuditoriumCreatedEventProjector(
            IRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = auditoriumReadModelRepository ?? throw new ArgumentNullException(nameof(auditoriumReadModelRepository));
        }

        public override async Task HandleAsync(AuditoriumCreatedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            var seats = @event.SeatIds
                .Select((seatId, index) => new SeatReadModel(
                    seatId,
                    @event.SeatRows[index],
                    @event.SeatNumbers[index]))
                .ToList();

            var auditoriumReadModel = new AuditoriumReadModel(
                @event.AggregateId,
                @event.Name,
                seats);

            await this.auditoriumReadModelRepository.AddAsync(auditoriumReadModel, cancellationToken);
        }
    }
}