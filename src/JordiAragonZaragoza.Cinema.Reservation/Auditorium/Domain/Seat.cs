namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;

    public sealed class Seat : BaseEntity<SeatId>
    {
        internal Seat(
            SeatId id,
            Row row,
            SeatNumber seatNumber)
            : base(id)
        {
            this.Row = Guard.Against.Null(row, nameof(row));
            this.SeatNumber = Guard.Against.Null(seatNumber, nameof(seatNumber));
        }

        // Required by EF
        private Seat()
        {
        }

        public Row Row { get; private set; } = default!;

        public SeatNumber SeatNumber { get; private set; } = default!;
    }
}