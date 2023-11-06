namespace JordiAragon.Cinemas.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Ticket : BaseEntity<TicketId>
    {
        private readonly List<SeatId> seats = new();

        private Ticket(
            TicketId id,
            IEnumerable<SeatId> seatIds,
            DateTime createdTimeOnUtc)
            : base(id)
        {
            this.seats = Guard.Against.NullOrEmpty(seatIds, nameof(seatIds)).ToList();
            this.CreatedTimeOnUtc = Guard.Against.Default(createdTimeOnUtc, nameof(createdTimeOnUtc));
        }

        // Required by EF.
        private Ticket()
        {
        }

        public IEnumerable<SeatId> Seats => this.seats.AsReadOnly();

        public DateTime CreatedTimeOnUtc { get; private set; }

        public bool IsPurchased { get; private set; }

        internal static Ticket Create(
            TicketId id,
            IEnumerable<SeatId> seatIds,
            DateTime createdTimeOnUtc)
        {
            return new Ticket(id, seatIds, createdTimeOnUtc);
        }

        internal void MarkAsPurchased()
        {
            if (!this.IsPurchased)
            {
                this.IsPurchased = true;
            }
        }
    }
}