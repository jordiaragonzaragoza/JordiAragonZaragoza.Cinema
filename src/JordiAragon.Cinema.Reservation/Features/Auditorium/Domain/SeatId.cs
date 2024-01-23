namespace JordiAragon.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

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