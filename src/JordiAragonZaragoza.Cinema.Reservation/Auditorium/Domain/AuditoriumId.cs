namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class AuditoriumId : BaseAggregateRootId<Guid>
    {
        public AuditoriumId(Guid value)
            : base(value)
        {
        }
    }
}