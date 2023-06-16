namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Specifications
{
    using System.Linq;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;

    public class ShowtimesByAuditoriumIdSpec : Specification<Showtime>
    {
        // TODO: Candidate to include a DateTime interval.
        public ShowtimesByAuditoriumIdSpec(AuditoriumId auditoriumId)
        {
            this.Query
                .Where(showtime => showtime.AuditoriumId == auditoriumId);
        }
    }
}