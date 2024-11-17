namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class ShowtimeId : BaseAggregateRootId<Guid>
    {
        public ShowtimeId(Guid value)
            : base(value)
        {
        }
    }
}