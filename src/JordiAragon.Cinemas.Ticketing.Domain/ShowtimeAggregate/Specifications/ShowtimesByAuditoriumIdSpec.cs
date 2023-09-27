namespace JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate.Specifications
{
    using System;
    using System.Linq;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.MovieAggregate;

    public class ShowtimesByAuditoriumIdSpec : Specification<Showtime>
    {
        public ShowtimesByAuditoriumIdSpec(
            AuditoriumId auditoriumId,
            DateTime? startTimeOnUtc,
            DateTime? endTimeOnUtc,
            MovieId movieId = null)
        {
            Guard.Against.Null(auditoriumId);

            // TODO: Complete the query.
            this.Query
                .Where(showtime => showtime.AuditoriumId == auditoriumId);
        }
    }
}