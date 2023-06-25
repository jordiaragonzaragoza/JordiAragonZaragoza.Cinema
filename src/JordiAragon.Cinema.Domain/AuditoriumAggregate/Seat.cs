namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Seat : BaseAuditableEntity<SeatId>
    {
        private Seat(
            SeatId id,
            short row,
            short seatNumber)
            : base(id)
        {
            this.Row = row; // TODO: Add Guard.
            this.SeatNumber = seatNumber;
        }

        // Required by EF
        private Seat()
            : base()
        {
        }

        public short Row { get; private set; }

        public short SeatNumber { get; private set; }

        public static Seat Create(
            SeatId seatId,
            short row,
            short seatNumber)
        {
            return new Seat(seatId, row, seatNumber);
        }
    }
}