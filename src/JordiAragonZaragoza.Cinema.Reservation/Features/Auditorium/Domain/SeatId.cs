namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class SeatId : BaseEntityId<Guid>
    {
        private SeatId(Guid value)
            : base(value)
        {
        }

        public static SeatId Create(Guid id)
            => new(id);
    }
}