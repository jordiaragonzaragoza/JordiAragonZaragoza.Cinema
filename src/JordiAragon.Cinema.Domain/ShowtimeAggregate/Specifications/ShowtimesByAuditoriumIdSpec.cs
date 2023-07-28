namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Specifications
{
    using System;
    using System.Linq;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;

    public class ShowtimesByAuditoriumIdSpec : Specification<Showtime>
    {
        public ShowtimesByAuditoriumIdSpec(
            AuditoriumId auditoriumId,
            MovieId movieId,
            DateTime? startTimeOnUtc,
            DateTime? endTimeOnUtc)
        {
            Guard.Against.Null(auditoriumId);
            Guard.Against.Null(movieId);
            ////Guard.Against.Default(startTimeOnUtc);

            // TODO: Complete the query.
            this.Query
                .Where(showtime => showtime.AuditoriumId == auditoriumId);
        }
    }
}