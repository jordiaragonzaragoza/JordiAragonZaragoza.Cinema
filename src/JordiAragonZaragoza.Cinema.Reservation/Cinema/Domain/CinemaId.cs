namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class CinemaId : BaseAggregateRootId<Guid>
    {
        public CinemaId(Guid value)
            : base(value)
        {
        }
    }
}