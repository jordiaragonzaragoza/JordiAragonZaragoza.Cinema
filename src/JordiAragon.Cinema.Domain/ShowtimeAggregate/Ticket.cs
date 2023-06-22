namespace JordiAragon.Cinema.Domain.ShowtimeAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Ticket : BaseAuditableEntity<TicketId>
    {
        private readonly List<SeatId> seats;

        private Ticket(
            TicketId id,
            ShowtimeId showtimeId,
            IEnumerable<SeatId> seatIds,
            DateTime createdTimeOnUtc)
            : base(id)
        {
            this.ShowtimeId = Guard.Against.Null(showtimeId, nameof(showtimeId));
            this.seats = Guard.Against.NullOrEmpty(seatIds, nameof(seatIds)).ToList();
            this.CreatedTimeOnUtc = createdTimeOnUtc;
        }

        // Required by EF.
        private Ticket()
        {
        }

        public ShowtimeId ShowtimeId { get; private set; }

        public IEnumerable<SeatId> Seats => this.seats.AsReadOnly();

        public DateTime CreatedTimeOnUtc { get; private set; }

        public bool IsPaid { get; private set; }

        public static Ticket Create(
            TicketId id,
            ShowtimeId showtimeId,
            IEnumerable<SeatId> seatIds,
            DateTime createdTimeOnUtc)
        {
            return new Ticket(id, showtimeId, seatIds, createdTimeOnUtc);
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