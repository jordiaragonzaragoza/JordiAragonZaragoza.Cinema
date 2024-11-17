namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class TicketId : BaseEntityId<Guid>
    {
        public TicketId(Guid value)
            : base(value)
        {
        }
    }
}