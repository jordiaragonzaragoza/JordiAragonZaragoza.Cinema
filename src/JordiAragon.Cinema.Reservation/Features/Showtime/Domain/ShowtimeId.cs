namespace JordiAragon.Cinema.Reservation.Showtime.Domain
{
    using System;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class ShowtimeId : BaseAggregateRootId<Guid>
    {
        private ShowtimeId(Guid value)
            : base(value)
        {
        }

        public static ShowtimeId Create(Guid id)
            => new(id);
    }
}