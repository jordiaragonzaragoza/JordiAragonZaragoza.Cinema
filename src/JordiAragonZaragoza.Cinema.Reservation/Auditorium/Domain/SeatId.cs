namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class SeatId : BaseEntityId<Guid>
    {
        public SeatId(Guid value)
            : base(value)
        {
        }
    }
}