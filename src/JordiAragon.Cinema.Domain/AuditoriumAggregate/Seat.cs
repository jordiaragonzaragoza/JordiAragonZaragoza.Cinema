namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Seat : BaseAuditableEntity<SeatId>
    {
        private Seat(
            SeatId id,
            short row,
            short seatNumber)
            : base(id)
        {
            Guard.Against.NegativeOrZero(row, nameof(row));
            Guard.Against.NegativeOrZero(seatNumber, nameof(seatNumber));

            this.Row = row;
            this.SeatNumber = seatNumber;
        }

        // Required by EF
        private Seat()
        {
        }

        public short Row { get; private set; }

        public short SeatNumber { get; private set; }

        internal static Seat Create(
            SeatId seatId,
            short row,
            short seatNumber)
        {
            return new Seat(seatId, row, seatNumber);
        }
    }
}