﻿namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;

    public sealed class Reservation : BaseEntity<ReservationId>
    {
        private readonly List<SeatId> seats = new();

        internal Reservation(
            ReservationId id,
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
        private Reservation()
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