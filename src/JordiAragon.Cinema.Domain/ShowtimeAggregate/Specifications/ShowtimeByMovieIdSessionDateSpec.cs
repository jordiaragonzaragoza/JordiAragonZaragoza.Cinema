namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Specifications
{
    using System;
    using System.Linq;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Domain.MovieAggregate;

    public class ShowtimeByMovieIdSessionDateSpec : SingleResultSpecification<Showtime>
    {
        public ShowtimeByMovieIdSessionDateSpec(MovieId movieId, DateTime sessionDateOnUtc)
        {
            Guard.Against.Null(movieId);
            Guard.Against.Default(sessionDateOnUtc);

            this.Query
                .Where(showtime => showtime.MovieId == movieId
                                && showtime.SessionDateOnUtc == sessionDateOnUtc);
        }
    }
}