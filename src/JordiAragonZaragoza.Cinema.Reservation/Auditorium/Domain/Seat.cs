namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;

    public sealed class Seat : BaseEntity<SeatId>
    {
        internal Seat(
            SeatId id,
            Row row,
            SeatNumber seatNumber)
            : base(id)
        {
            this.Row = row ?? throw new ArgumentNullException(nameof(row));
            this.SeatNumber = seatNumber ?? throw new ArgumentNullException(nameof(seatNumber));
        }

        // Required by EF
        private Seat()
        {
        }

        public Row Row { get; private set; } = default!;

        public SeatNumber SeatNumber { get; private set; } = default!;
    }
}