namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class TicketId : BaseEntityId<Guid>
    {
        private TicketId(Guid value)
            : base(value)
        {
        }

        public static TicketId Create(Guid id)
            => new(id);
    }
}