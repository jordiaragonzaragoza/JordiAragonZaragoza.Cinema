namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications
{
    using Ardalis.Specification;

    public class AuditoriumWithShowtimesByIdSpec : SingleResultSpecification<Auditorium>
    {
        public AuditoriumWithShowtimesByIdSpec(AuditoriumId id)
        {
            this.Query
                .Where(auditorium => auditorium.Id == id)
                .Include(auditorium => auditorium.Showtimes);
        }
    }
}