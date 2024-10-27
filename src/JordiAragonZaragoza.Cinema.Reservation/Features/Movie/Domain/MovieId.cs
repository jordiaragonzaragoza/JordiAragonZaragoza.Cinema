namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class MovieId : BaseAggregateRootId<Guid>
    {
        private MovieId(Guid value)
            : base(value)
        {
        }

        public static MovieId Create(Guid id)
            => new(id);
    }
}