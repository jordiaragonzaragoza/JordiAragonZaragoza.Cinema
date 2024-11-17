namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class MovieId : BaseAggregateRootId<Guid>
    {
        public MovieId(Guid value)
            : base(value)
        {
        }
    }
}