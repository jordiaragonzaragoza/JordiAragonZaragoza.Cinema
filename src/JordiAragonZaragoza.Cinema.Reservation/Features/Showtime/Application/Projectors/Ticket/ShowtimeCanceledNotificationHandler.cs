namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Ticket
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class ShowtimeCanceledNotificationHandler : IEventNotificationHandler<ShowtimeCanceledNotification>
    {
        private readonly IRangeableRepository<TicketReadModel, Guid> ticketReadModelRepository;
        private readonly ISpecificationReadRepository<TicketReadModel, Guid> specificationRepository;

        public ShowtimeCanceledNotificationHandler(
            IRangeableRepository<TicketReadModel, Guid> ticketReadModelRepository,
            ISpecificationReadRepository<TicketReadModel, Guid> specificationRepository)
        {
            this.ticketReadModelRepository = Guard.Against.Null(ticketReadModelRepository, nameof(ticketReadModelRepository));
            this.specificationRepository = Guard.Against.Null(specificationRepository, nameof(specificationRepository));
        }

        public async Task Handle(ShowtimeCanceledNotification notification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(notification, nameof(notification));

            var @event = notification.Event;

            var specification = new GetTicketsByShowtimeIdSpec(@event.AggregateId);
            var existingTickets = await this.specificationRepository.ListAsync(specification, cancellationToken);
            if (existingTickets.Count > 0)
            {
                await this.ticketReadModelRepository.DeleteRangeAsync(existingTickets, cancellationToken);
            }
        }
    }
}