namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Seat : BaseAuditableEntity<SeatId>
    {
        private Seat(
            SeatId id,
            AuditoriumId auditoriumId,
            short row,
            short seatNumber)
            : base(id)
        {
            this.AuditoriumId = Guard.Against.Null(auditoriumId, nameof(auditoriumId));
            this.Row = row;
            this.SeatNumber = seatNumber;
        }

        // Required by EF
        private Seat()
        {
        }

        public AuditoriumId AuditoriumId { get; private set; } // TODO: Review. Is it required?

        public short Row { get; private set; }

        public short SeatNumber { get; private set; }

        public static Seat Create(
            SeatId seatId,
            AuditoriumId auditoriumId,
            short row,
            short seatNumber)
        {
            return new Seat(seatId, auditoriumId, row, seatNumber);
        }
    }
}