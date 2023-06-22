namespace JordiAragon.Cinema.Domain.ShowtimeAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Events;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Rules;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class Showtime : BaseAggregateRoot<ShowtimeId, Guid>, IAggregateRoot
    {
        private readonly List<Ticket> tickets = new();

        private Showtime(
            ShowtimeId id,
            MovieId movieId,
            DateTime sessionDateOnUtc,
            AuditoriumId auditoriumId)
            : base(id)
        {
            this.MovieId = Guard.Against.Null(movieId, nameof(movieId));
            this.SessionDateOnUtc = sessionDateOnUtc;
            this.AuditoriumId = Guard.Against.Null(auditoriumId, nameof(auditoriumId));

            this.RegisterDomainEvent(new ShowtimeCreatedEvent(id, this.MovieId, this.SessionDateOnUtc, this.AuditoriumId));
        }

        // Required by EF.
        private Showtime()
        {
        }

        public MovieId MovieId { get; private set; }

        public DateTime SessionDateOnUtc { get; private set; }

        public AuditoriumId AuditoriumId { get; private set; }

        public IReadOnlyList<Ticket> Tickets => this.tickets.AsReadOnly();

        public static Showtime Create(
            ShowtimeId id,
            MovieId movieId,
            DateTime sessionDateOnUtc,
            AuditoriumId auditoriumId)
        {
            return new Showtime(id, movieId, sessionDateOnUtc, auditoriumId);
        }

        public void PurchaseSeats(TicketId ticketId)
        {
            var ticket = this.Tickets.FirstOrDefault(ticket => ticket.Id == ticketId)
                ?? throw new NotFoundException(nameof(Ticket), ticketId.Value);

            CheckRule(new OnlyPossibleToPayOncePerTicketRule(ticket));

            ticket.MarkAsPaid();

            var @event = new PurchasedSeatsEvent(ticketId);
            this.RegisterDomainEvent(@event);
        }

        public void ExpireReservedSeats(TicketId ticketToRemove)
        {
            Guard.Against.Null(ticketToRemove, nameof(ticketToRemove));

            var existingTicket = this.Tickets.FirstOrDefault(item => item.Id == ticketToRemove)
                                   ?? throw new NotFoundException(nameof(Ticket), ticketToRemove.Value.ToString());

            this.tickets.Remove(existingTicket);

            var @event = new ExpiredReservedSeatsEvent(ticketToRemove);
            this.RegisterDomainEvent(@event);
        }

        public Ticket ReserveSeats(TicketId id, IEnumerable<SeatId> seatIds, DateTime createdTimeOnUtc)
        {
            var newTicket = Ticket.Create(
                 id,
                 ShowtimeId.Create(this.Id.Value),
                 seatIds,
                 createdTimeOnUtc);

            this.tickets.Add(newTicket);

            this.RegisterDomainEvent(new ReservedSeatsEvent(id, seatIds, createdTimeOnUtc));

            return newTicket;
        }
    }
}