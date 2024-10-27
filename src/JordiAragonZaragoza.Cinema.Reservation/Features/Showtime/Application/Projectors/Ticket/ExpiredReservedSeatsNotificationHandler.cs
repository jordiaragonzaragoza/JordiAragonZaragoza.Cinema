namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Ticket
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ExpiredReservedSeatsNotificationHandler : IEventNotificationHandler<ExpiredReservedSeatsNotification>
    {
        private readonly IRepository<TicketReadModel, Guid> ticketReadModelRepository;

        public ExpiredReservedSeatsNotificationHandler(
            IRepository<TicketReadModel, Guid> ticketReadModelRepository)
        {
            this.ticketReadModelRepository = Guard.Against.Null(ticketReadModelRepository, nameof(ticketReadModelRepository));
        }

        public async Task Handle(ExpiredReservedSeatsNotification notification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(notification, nameof(notification));

            var @event = notification.Event;

            var existingTicket = await this.ticketReadModelRepository.GetByIdAsync(@event.TicketId, cancellationToken);
            if (existingTicket is null)
            {
                throw new NotFoundException(nameof(TicketReadModel), @event.TicketId.ToString());
            }

            await this.ticketReadModelRepository.DeleteAsync(existingTicket, cancellationToken);
        }
    }
}