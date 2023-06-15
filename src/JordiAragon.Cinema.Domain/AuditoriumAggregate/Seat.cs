namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Seat : BaseAuditableEntity<SeatId>
    {
        private readonly List<TicketSeat> tickets = new();

        private Seat(
            SeatId id,
            AuditoriumId auditoriumId,
            short row,
            short seatNumber)
            : this(id)
        {
            this.AuditoriumId = Guard.Against.Null(auditoriumId, nameof(auditoriumId));
            this.Row = row;
            this.SeatNumber = seatNumber;
        }

        // Required by EF.
        private Seat(
            SeatId id)
            : base(id)
        {
        }

        public IEnumerable<TicketSeat> Tickets => this.tickets.AsReadOnly();

        public AuditoriumId AuditoriumId { get; private set; }

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