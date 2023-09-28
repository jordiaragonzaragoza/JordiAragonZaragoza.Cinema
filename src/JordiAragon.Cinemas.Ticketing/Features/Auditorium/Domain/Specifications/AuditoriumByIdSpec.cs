namespace JordiAragon.Cinemas.Ticketing.Auditorium.Domain.Specifications
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;

    public class AuditoriumByIdSpec : SingleResultSpecification<Auditorium>
    {
        public AuditoriumByIdSpec(AuditoriumId auditoriumId)
        {
            Guard.Against.Null(auditoriumId);

            this.Query
                .Where(auditorium => auditorium.Id == auditoriumId);
        }
    }
}