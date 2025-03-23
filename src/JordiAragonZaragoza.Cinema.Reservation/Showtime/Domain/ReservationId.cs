namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class ReservationId : BaseEntityId<Guid>
    {
        public ReservationId(Guid value)
            : base(value)
        {
        }
    }
}