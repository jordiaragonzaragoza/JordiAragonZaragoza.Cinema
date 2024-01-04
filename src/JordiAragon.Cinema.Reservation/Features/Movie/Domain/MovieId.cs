namespace JordiAragon.Cinema.Reservation.Movie.Domain
{
    using System;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

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