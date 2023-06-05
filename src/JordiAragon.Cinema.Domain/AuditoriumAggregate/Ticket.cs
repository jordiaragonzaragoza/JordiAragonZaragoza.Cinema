namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Events;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Ticket : BaseAuditableEntity<TicketId>
    {
        private readonly List<TicketSeat> seats;

        private Ticket(
            TicketId id,
            ShowtimeId showtimeId,
            List<TicketSeat> seats,
            DateTime createdTimeOnUtc)
            : this(id)
        {
            this.ShowtimeId = Guard.Against.Null(showtimeId, nameof(showtimeId));
            this.seats = Guard.Against.NullOrEmpty(seats, nameof(seats)).ToList();
            this.CreatedTimeOnUtc = createdTimeOnUtc;
        }

        // Required by EF.
        private Ticket(
            TicketId id)
            : base(id)
        {
        }

        public ShowtimeId ShowtimeId { get; private set; }

        public IEnumerable<TicketSeat> Seats => this.seats.AsReadOnly();

        public DateTime CreatedTimeOnUtc { get; private set; }

        public bool IsPaid { get; private set; }

        public static Ticket Create(
            TicketId id,
            ShowtimeId showtimeId,
            IEnumerable<Seat> seatsData,
            DateTime createdTimeOnUtc)
        {
            Guard.Against.Null(id, nameof(id));
            Guard.Against.NullOrEmpty(seatsData, nameof(seatsData));

            var ticketSeats = new List<TicketSeat>();

            foreach (var seat in seatsData)
            {
                ticketSeats.Add(new TicketSeat(TicketSeatId.CreateUnique(), seat.Id, id));
            }

            return new Ticket(id, showtimeId, ticketSeats, createdTimeOnUtc);
        }

        public void MarkAsPaid()
        {
            if (!this.IsPaid)
            {
                this.IsPaid = true;
            }
        }
    }
}