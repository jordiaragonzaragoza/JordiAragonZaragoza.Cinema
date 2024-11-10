namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class Showtime : BaseAggregateRoot<ShowtimeId, Guid>
    {
        private readonly List<Ticket> tickets = new();

        // Required by EF.
        private Showtime()
        {
        }

        public MovieId MovieId { get; private set; } = default!;

        public SessionDate SessionDateOnUtc { get; private set; } = default!;

        public AuditoriumId AuditoriumId { get; private set; } = default!;

        public bool IsEnded { get; private set; } // TODO: Review. Probably this property is not required.

        public IEnumerable<Ticket> Tickets => this.tickets.AsReadOnly();

        public static Showtime Schedule(
            ShowtimeId id,
            MovieId movieId,
            SessionDate sessionDateOnUtc,
            AuditoriumId auditoriumId)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(movieId, nameof(movieId));
            ArgumentNullException.ThrowIfNull(sessionDateOnUtc, nameof(sessionDateOnUtc));
            ArgumentNullException.ThrowIfNull(auditoriumId, nameof(auditoriumId));

            var showtime = new Showtime();

            showtime.Apply(new ShowtimeScheduledEvent(id, movieId, sessionDateOnUtc, auditoriumId));

            return showtime;
        }

        public void Cancel()
            => this.Apply(new ShowtimeCanceledEvent(this.Id, this.AuditoriumId, this.MovieId));

        public void End()
            => this.Apply(new ShowtimeEndedEvent(this.Id, this.AuditoriumId, this.MovieId));

        public Ticket ReserveSeats(TicketId id, UserId userId, IEnumerable<SeatId> seatIds, ReservationDate reservationDateOnUtc)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            Guard.Against.NullOrEmpty(seatIds, nameof(seatIds));
            ArgumentNullException.ThrowIfNull(reservationDateOnUtc, nameof(reservationDateOnUtc));

            this.Apply(new ReservedSeatsEvent(this.Id, id, userId, seatIds.Select(x => x.Value), reservationDateOnUtc));

            return this.tickets[^1];
        }

        public void PurchaseTicket(TicketId ticketId)
        {
            ArgumentNullException.ThrowIfNull(ticketId, nameof(ticketId));

            var ticket = this.Tickets.FirstOrDefault(ticket => ticket.Id == ticketId)
                         ?? throw new NotFoundException(nameof(Ticket), ticketId.Value);

            CheckRule(new OnlyPossibleToPurchaseOncePerTicketRule(ticket));

            this.Apply(new PurchasedTicketEvent(this.Id, ticketId));
        }

        public void ExpireReservedSeats(TicketId ticketToRemove)
        {
            ArgumentNullException.ThrowIfNull(ticketToRemove, nameof(ticketToRemove));

            _ = this.Tickets.FirstOrDefault(item => item.Id == ticketToRemove)
                ?? throw new NotFoundException(nameof(Ticket), ticketToRemove.Value);

            this.Apply(new ExpiredReservedSeatsEvent(this.Id, ticketToRemove));
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case ShowtimeScheduledEvent @event:
                    this.Applier(@event);
                    break;

                case ShowtimeCanceledEvent:
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

                default:
                    throw new EventCannotBeAppliedToAggregateException<Showtime, ShowtimeId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            try
            {
                Guard.Against.Null(this.Id, nameof(this.Id));
                Guard.Against.Null(this.MovieId, nameof(this.MovieId));
                Guard.Against.Null(this.SessionDateOnUtc, nameof(this.SessionDateOnUtc));
                Guard.Against.Null(this.AuditoriumId, nameof(this.AuditoriumId));
            }
            catch (Exception exception)
            {
                throw new InvalidAggregateStateException<Showtime, ShowtimeId>(this, exception.Message);
            }
        }

        private void Applier(ShowtimeScheduledEvent @event)
        {
            this.Id = new ShowtimeId(@event.AggregateId);
            this.MovieId = new MovieId(@event.MovieId);
            this.SessionDateOnUtc = new SessionDate(@event.SessionDateOnUtc);
            this.AuditoriumId = new AuditoriumId(@event.AuditoriumId);
        }

        private void Applier()
            => this.IsEnded = true;

        private void Applier(ReservedSeatsEvent @event)
        {
            var seatIds = @event.SeatIds.Select(i => new SeatId(i));

            var newTicket = new Ticket(
                 new TicketId(@event.TicketId),
                 new UserId(@event.UserId),
                 seatIds,
                 new ReservationDate(@event.CreatedTimeOnUtc));

            this.tickets.Add(newTicket);
        }

        private void Applier(PurchasedTicketEvent @event)
        {
            var ticketId = new TicketId(@event.TicketId);

            var ticket = this.Tickets.FirstOrDefault(ticket => ticket.Id == ticketId);

            ticket?.MarkAsPurchased();
        }

        private void Applier(ExpiredReservedSeatsEvent @event)
        {
            var ticketToRemove = new TicketId(@event.TicketId);

            var existingTicket = this.Tickets.FirstOrDefault(item => item.Id == ticketToRemove);

            this.tickets.Remove(existingTicket!);
        }
    }
}