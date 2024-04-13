namespace JordiAragon.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Domain.Exceptions;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class Showtime : BaseAggregateRoot<ShowtimeId, Guid>
    {
        private readonly List<Ticket> tickets = new();

        // Required by EF.
        private Showtime()
        {
        }

        public MovieId MovieId { get; private set; }

        public DateTimeOffset SessionDateOnUtc { get; private set; }

        public AuditoriumId AuditoriumId { get; private set; }

        public bool IsEnded { get; private set; }

        public IEnumerable<Ticket> Tickets => this.tickets.AsReadOnly();

        public static Showtime Schedule(
            ShowtimeId id,
            MovieId movieId,
            DateTimeOffset sessionDateOnUtc,
            AuditoriumId auditoriumId)
        {
            var showtime = new Showtime();

            showtime.Apply(new ShowtimeScheduledEvent(id, movieId, sessionDateOnUtc, auditoriumId));

            return showtime;
        }

        public void End()
            => this.Apply(new ShowtimeEndedEvent(this.Id));

        public Ticket ReserveSeats(TicketId id, UserId userId, IEnumerable<SeatId> seatIds, DateTimeOffset createdTimeOnUtc)
        {
            this.Apply(new ReservedSeatsEvent(this.Id, id, userId, seatIds.Select(x => x.Value), createdTimeOnUtc));

            return this.tickets[this.tickets.Count - 1];
        }

        public void PurchaseTicket(TicketId ticketId)
        {
            this.Apply(new PurchasedTicketEvent(this.Id, ticketId));
        }

        public void ExpireReservedSeats(TicketId ticketToRemove)
            => this.Apply(new ExpiredReservedSeatsEvent(this.Id, ticketToRemove));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case ShowtimeScheduledEvent @event:
                    this.Applier(@event);
                    break;

                case ShowtimeEndedEvent:
                    this.Applier();
                    break;

                case ReservedSeatsEvent @event:
                    this.Applier(@event);
                    break;

                case PurchasedTicketEvent @event:
                    this.Applier(@event);
                    break;

                case ExpiredReservedSeatsEvent @event:
                    this.Applier(@event);
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            try
            {
                Guard.Against.Null(this.Id, nameof(this.Id));
                Guard.Against.Null(this.MovieId, nameof(this.MovieId));
                Guard.Against.Default(this.SessionDateOnUtc, nameof(this.SessionDateOnUtc));
                Guard.Against.Null(this.AuditoriumId, nameof(this.AuditoriumId));
            }
            catch (Exception exception)
            {
                throw new InvalidAggregateStateException<Showtime, ShowtimeId>(this, exception.Message);
            }
        }

        private void Applier(ShowtimeScheduledEvent @event)
        {
            this.Id = ShowtimeId.Create(@event.AggregateId);
            this.MovieId = MovieId.Create(@event.MovieId);
            this.SessionDateOnUtc = @event.SessionDateOnUtc;
            this.AuditoriumId = AuditoriumId.Create(@event.AuditoriumId);
        }

        private void Applier()
            => this.IsEnded = true;

        private void Applier(ReservedSeatsEvent @event)
        {
            var seatIds = @event.SeatIds.Select(SeatId.Create);

            var newTicket = Ticket.Create(
                 TicketId.Create(@event.TicketId),
                 UserId.Create(@event.UserId),
                 seatIds,
                 @event.CreatedTimeOnUtc);

            this.tickets.Add(newTicket);
        }

        private void Applier(PurchasedTicketEvent @event)
        {
            var ticketId = TicketId.Create(@event.TicketId);

            var ticket = this.Tickets.FirstOrDefault(ticket => ticket.Id == ticketId)
                ?? throw new NotFoundException(nameof(Ticket), ticketId.Value);

            CheckRule(new OnlyPossibleToPurchaseOncePerTicketRule(ticket));

            ticket.MarkAsPurchased();
        }

        private void Applier(ExpiredReservedSeatsEvent @event)
        {
            var ticketToRemove = TicketId.Create(@event.TicketId);

            var existingTicket = this.Tickets.FirstOrDefault(item => item.Id == ticketToRemove)
                                   ?? throw new NotFoundException(nameof(Ticket), ticketToRemove.Value.ToString());

            this.tickets.Remove(existingTicket);
        }
    }
}