namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projections.AvailableSeat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using MediatR;
    using Volo.Abp.Guids;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ExpiredReservedSeatsNotificationHandler : INotificationHandler<ExpiredReservedSeatsNotification>
    {
        private readonly IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository;
        private readonly IReadRepository<Showtime, ShowtimeId> showtimeReadRepository;
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumReadRepository;
        private readonly IGuidGenerator guidGenerator;

        public ExpiredReservedSeatsNotificationHandler(
            IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository,
            IReadRepository<Showtime, ShowtimeId> showtimeReadRepository,
            IReadRepository<Auditorium, AuditoriumId> auditoriumReadRepository,
            IGuidGenerator guidGenerator)
        {
            this.availableReadModelRepository = Guard.Against.Null(availableReadModelRepository, nameof(availableReadModelRepository));
            this.showtimeReadRepository = Guard.Against.Null(showtimeReadRepository, nameof(showtimeReadRepository));
            this.auditoriumReadRepository = Guard.Against.Null(auditoriumReadRepository, nameof(auditoriumReadRepository));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public async Task Handle(ExpiredReservedSeatsNotification notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            var existingShowtime = await this.showtimeReadRepository.GetByIdAsync(ShowtimeId.Create(@event.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                throw new NotFoundException(nameof(Showtime), @event.ShowtimeId.ToString());
            }

            var ticket = existingShowtime.Tickets.FirstOrDefault(ticket => ticket.Id == @event.TicketId);
            if (ticket is null)
            {
                throw new NotFoundException(nameof(Ticket), @event.TicketId.ToString());
            }

            var existingAuditorium = await this.auditoriumReadRepository.GetByIdAsync(AuditoriumId.Create(existingShowtime.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), existingShowtime.AuditoriumId.ToString());
            }

            var newAvailableSeats = existingAuditorium.Seats.Where(seat => ticket.Seats.Contains(seat.Id));
            var availableSeats = new List<AvailableSeatReadModel>();
            foreach (var seat in newAvailableSeats)
            {
                availableSeats.Add(new AvailableSeatReadModel(
                    this.guidGenerator.Create(),
                    seat.Id,
                    seat.Row,
                    seat.SeatNumber,
                    @event.ShowtimeId,
                    existingAuditorium.Id,
                    existingAuditorium.Name));
            }

            await this.availableReadModelRepository.AddRangeAsync(availableSeats, cancellationToken);
        }
    }
}