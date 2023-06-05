namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class TicketSeat : BaseAuditableEntity<TicketSeatId>
    {
        public TicketSeat(
            TicketSeatId id,
            SeatId seatId,
            TicketId ticketId)
            : this(id)
        {
            this.SeatId = Guard.Against.Null(seatId, nameof(seatId));
            this.TicketId = Guard.Against.Null(ticketId, nameof(ticketId));
        }

        // Required by EF.
        private TicketSeat(
            TicketSeatId id)
            : base(id)
        {
        }

        public SeatId SeatId { get; private set; }

        public TicketId TicketId { get; private set; }
    }
}