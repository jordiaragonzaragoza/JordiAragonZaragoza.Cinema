namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Projectors.Auditorium
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Notifications;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class AuditoriumRemovedNotificationHandler : IEventNotificationHandler<AuditoriumRemovedNotification>
    {
        private readonly IRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository;

        public AuditoriumRemovedNotificationHandler(
            IRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = Guard.Against.Null(auditoriumReadModelRepository, nameof(auditoriumReadModelRepository));
        }

        public async Task Handle(AuditoriumRemovedNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var @event = notification.Event;

            var readModel = await this.auditoriumReadModelRepository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(AuditoriumReadModel), @event.AggregateId.ToString());
            }

            await this.auditoriumReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}