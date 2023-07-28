namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Specifications
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;

    public class ShowtimeByIdSpec : SingleResultSpecification<Showtime>
    {
        public ShowtimeByIdSpec(ShowtimeId showtimeId)
        {
            Guard.Against.Null(showtimeId);

            this.Query
                .Where(showtime => showtime.Id == showtimeId);
        }
    }
}