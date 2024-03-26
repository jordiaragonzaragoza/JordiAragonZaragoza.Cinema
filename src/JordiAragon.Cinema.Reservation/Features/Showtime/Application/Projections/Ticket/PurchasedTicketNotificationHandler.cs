namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projections.Ticket
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using MediatR;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class PurchasedTicketNotificationHandler : INotificationHandler<PurchasedTicketNotification>
    {
        private readonly IRepository<TicketReadModel, Guid> ticketReadModelRepository;

        public PurchasedTicketNotificationHandler(
            IRepository<TicketReadModel, Guid> ticketReadModelRepository)
        {
            this.ticketReadModelRepository = Guard.Against.Null(ticketReadModelRepository, nameof(ticketReadModelRepository));
        }

        public async Task Handle(PurchasedTicketNotification notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            var existingTicket = await this.ticketReadModelRepository.GetByIdAsync(@event.TicketId, cancellationToken);
            if (existingTicket is null)
            {
                throw new NotFoundException(nameof(TicketReadModel), @event.TicketId.ToString());
            }

            existingTicket.IsPurchased = true;

            await this.ticketReadModelRepository.UpdateAsync(existingTicket, cancellationToken);
        }
    }
}