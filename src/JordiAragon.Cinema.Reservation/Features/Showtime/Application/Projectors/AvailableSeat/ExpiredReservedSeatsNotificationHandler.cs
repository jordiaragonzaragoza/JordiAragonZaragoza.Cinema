namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projectors.AvailableSeat
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ExpiredReservedSeatsNotificationHandler : IEventNotificationHandler<ExpiredReservedSeatsNotification>
    {
        private readonly IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository;
        private readonly ISpecificationReadRepository<AvailableSeatReadModel, Guid> specificationReadRepository;
        private readonly IReadRepository<Showtime, ShowtimeId> showtimeReadRepository;
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumReadRepository;
        private readonly IIdGenerator guidGenerator;

        public ExpiredReservedSeatsNotificationHandler(
            IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository,
            ISpecificationReadRepository<AvailableSeatReadModel, Guid> specificationReadRepository,
            IReadRepository<Showtime, ShowtimeId> showtimeReadRepository,
            IReadRepository<Auditorium, AuditoriumId> auditoriumReadRepository,
            IIdGenerator guidGenerator)
        {
            this.availableReadModelRepository = Guard.Against.Null(availableReadModelRepository, nameof(availableReadModelRepository));
            this.specificationReadRepository = Guard.Against.Null(specificationReadRepository, nameof(specificationReadRepository));
            this.showtimeReadRepository = Guard.Against.Null(showtimeReadRepository, nameof(showtimeReadRepository));
            this.auditoriumReadRepository = Guard.Against.Null(auditoriumReadRepository, nameof(auditoriumReadRepository));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public async Task Handle(ExpiredReservedSeatsNotification notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            var existingShowtime = await this.showtimeReadRepository.GetByIdAsync(ShowtimeId.Create(@event.AggregateId), cancellationToken);
            if (existingShowtime is null)
            {
                throw new NotFoundException(nameof(Showtime), @event.AggregateId.ToString());
            }

            var existingAuditorium = await this.auditoriumReadRepository.GetByIdAsync(AuditoriumId.Create(existingShowtime.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), existingShowtime.AuditoriumId.ToString()!);
            }

            var specification = new GetAvailableSeatsByShowtimeIdSpec(existingShowtime.Id);
            var existingAvailableSeats = await this.specificationReadRepository.ListAsync(specification, cancellationToken);

            var existingAvailableSeatsIds = existingAvailableSeats.Select(seatReadModel => seatReadModel.SeatId);

            var totalAvailableSeats = ReservationManager.AvailableSeats(existingAuditorium, existingShowtime);

            var newAvailableSeats = totalAvailableSeats.Where(seat => !existingAvailableSeatsIds.Contains(seat.Id))
                                    .Select(seat => new AvailableSeatReadModel(
                                        this.guidGenerator.Create(),
                                        seat.Id,
                                        seat.Row,
                                        seat.SeatNumber,
                                        @event.AggregateId,
                                        existingAuditorium.Id,
                                        existingAuditorium.Name));

            await this.availableReadModelRepository.AddRangeAsync(newAvailableSeats, cancellationToken);
        }
    }
}