namespace JordiAragon.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class AuditoriumId : BaseAggregateRootId<Guid>
    {
        private AuditoriumId(Guid value)
            : base(value)
        {
        }

        public static AuditoriumId Create(Guid id)
            => new(id);
    }
}