namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications
{
    using Ardalis.Specification;

    public class AuditoriumByIdSpec : SingleResultSpecification<Auditorium>
    {
        public AuditoriumByIdSpec(AuditoriumId auditoriumId)
        {
            this.Query
                .Where(auditorium => auditorium.Id == auditoriumId);
        }
    }
}