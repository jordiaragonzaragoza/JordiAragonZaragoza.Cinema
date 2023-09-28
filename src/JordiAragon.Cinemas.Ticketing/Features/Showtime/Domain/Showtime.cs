namespace JordiAragon.Cinemas.Ticketing.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Domain;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain.Events;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain.Rules;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class Showtime : BaseAggregateRoot<ShowtimeId, Guid>
    {
        private readonly List<Ticket> tickets = new();

        // Required by EF.
        private Showtime()
        {
        }

        public MovieId MovieId { get; private set; }

        public DateTime SessionDateOnUtc { get; private set; }

        public AuditoriumId AuditoriumId { get; private set; }

        public IEnumerable<Ticket> Tickets => this.tickets.AsReadOnly();

        public static Showtime Create(
            ShowtimeId id,
            MovieId movieId,
            DateTime sessionDateOnUtc,
            AuditoriumId auditoriumId)
        {
            var showtime = new Showtime();

            showtime.Apply(new ShowtimeCreatedEvent(id, movieId, sessionDateOnUtc, auditoriumId));

            return showtime;
        }

        public Ticket ReserveSeats(TicketId id, IEnumerable<SeatId> seatIds, DateTime createdTimeOnUtc)
        {
            this.Apply(new ReservedSeatsEvent(this.Id, id, seatIds.Select(x => x.Value), createdTimeOnUtc));

            return this.tickets.Last();
        }

        public void PurchaseSeats(TicketId ticketId)
            => this.Apply(new PurchasedSeatsEvent(this.Id, ticketId));

        public void ExpireReservedSeats(TicketId ticketToRemove)
            => this.Apply(new ExpiredReservedSeatsEvent(this.Id, ticketToRemove));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case ShowtimeCreatedEvent @event:
                    this.Applier(@event);
                    break;

                case ReservedSeatsEvent @event:
                    this.Applier(@event);
                    break;

                case PurchasedSeatsEvent @event:
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
                throw new InvalidAggregateStateException(this, exception.Message);
            }
        }

        private void Applier(ShowtimeCreatedEvent @event)
        {
            this.Id = ShowtimeId.Create(@event.AggregateId);
            this.MovieId = MovieId.Create(@event.MovieId);
            this.SessionDateOnUtc = @event.SessionDateOnUtc;
            this.AuditoriumId = AuditoriumId.Create(@event.AuditoriumId);
        }

        private void Applier(ReservedSeatsEvent @event)
        {
            var seatIds = @event.SeatIds.Select(SeatId.Create);

            var newTicket = Ticket.Create(
                 TicketId.Create(@event.TicketId),
                 seatIds,
                 @event.CreatedTimeOnUtc);

            this.tickets.Add(newTicket);
        }

        private void Applier(PurchasedSeatsEvent @event)
        {
            var ticketId = TicketId.Create(@event.TicketId);

            var ticket = this.Tickets.FirstOrDefault(ticket => ticket.Id == ticketId)
                ?? throw new NotFoundException(nameof(Ticket), ticketId.Value);

            CheckRule(new OnlyPossibleToPayOncePerTicketRule(ticket));

            ticket.MarkAsPaid();
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