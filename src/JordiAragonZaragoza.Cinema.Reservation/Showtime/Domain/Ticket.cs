namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;

    public sealed class Ticket : BaseEntity<TicketId>
    {
        private readonly List<SeatId> seats = new();

        internal Ticket(
            TicketId id,
            UserId userId,
            IEnumerable<SeatId> seatIds,
            ReservationDate reservationDateOnUtc)
            : base(id)
        {
            this.UserId = userId;
            this.seats = seatIds.ToList();
            this.ReservationDateOnUtc = reservationDateOnUtc;
        }

        // Required by EF.
        private Ticket()
        {
        }

        public UserId UserId { get; private set; } = default!;

        public IEnumerable<SeatId> Seats => this.seats.AsReadOnly();

        public ReservationDate ReservationDateOnUtc { get; private set; } = default!;

        public bool IsPurchased { get; private set; }

        internal void MarkAsPurchased()
        {
            if (!this.IsPurchased)
            {
                this.IsPurchased = true;
            }
        }
    }
}