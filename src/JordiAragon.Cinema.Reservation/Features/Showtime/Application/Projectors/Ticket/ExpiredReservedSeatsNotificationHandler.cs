namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projectors.Ticket
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

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