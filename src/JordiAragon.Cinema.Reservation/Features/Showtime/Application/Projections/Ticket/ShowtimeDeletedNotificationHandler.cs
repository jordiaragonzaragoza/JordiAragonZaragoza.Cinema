﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projections.Ticket
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using MediatR;

    public sealed class ShowtimeDeletedNotificationHandler : INotificationHandler<ShowtimeDeletedNotification>
    {
        private readonly IRangeableRepository<TicketReadModel, Guid> ticketReadModelRepository;
        private readonly ISpecificationReadRepository<TicketReadModel, Guid> specificationRepository;

        public ShowtimeDeletedNotificationHandler(
            IRangeableRepository<TicketReadModel, Guid> ticketReadModelRepository,
            ISpecificationReadRepository<TicketReadModel, Guid> specificationRepository)
        {
            this.ticketReadModelRepository = Guard.Against.Null(ticketReadModelRepository, nameof(ticketReadModelRepository));
            this.specificationRepository = Guard.Against.Null(specificationRepository, nameof(specificationRepository));
        }

        public async Task Handle(ShowtimeDeletedNotification notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            var specification = new GetTicketsByShowtimeIdSpec(@event.ShowtimeId);
            var existingTickets = await this.specificationRepository.ListAsync(specification, cancellationToken);
            if (existingTickets.Any())
            {
                await this.ticketReadModelRepository.DeleteRangeAsync(existingTickets, cancellationToken);
            }
        }
    }
}