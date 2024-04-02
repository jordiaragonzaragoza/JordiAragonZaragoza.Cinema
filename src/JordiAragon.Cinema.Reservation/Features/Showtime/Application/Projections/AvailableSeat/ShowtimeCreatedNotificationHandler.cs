namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projections.AvailableSeat
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using MediatR;
    using Volo.Abp.Guids;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeCreatedNotificationHandler : INotificationHandler<ShowtimeCreatedNotification>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository;
        private readonly IGuidGenerator guidGenerator;

        public ShowtimeCreatedNotificationHandler(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository,
            IGuidGenerator guidGenerator)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.availableReadModelRepository = Guard.Against.Null(availableReadModelRepository, nameof(availableReadModelRepository));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public async Task Handle(ShowtimeCreatedNotification notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(@event.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), @event.AuditoriumId.ToString());
            }

            var availableSeats = new List<AvailableSeatReadModel>();
            foreach (var seat in existingAuditorium.Seats)
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