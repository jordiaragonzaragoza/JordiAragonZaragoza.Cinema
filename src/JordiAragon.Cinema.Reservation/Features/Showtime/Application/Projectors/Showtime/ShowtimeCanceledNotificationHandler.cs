namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projectors.Showtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using MediatR;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeCanceledNotificationHandler : INotificationHandler<ShowtimeCanceledNotification>
    {
        private readonly IRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository;

        public ShowtimeCanceledNotificationHandler(
            IRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository)
        {
            this.showtimeReadModelRepository = Guard.Against.Null(showtimeReadModelRepository, nameof(showtimeReadModelRepository));
        }

        public async Task Handle(ShowtimeCanceledNotification notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            var readModel = await this.showtimeReadModelRepository.GetByIdAsync(@event.ShowtimeId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(ShowtimeReadModel), @event.ShowtimeId.ToString());
            }

            await this.showtimeReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}