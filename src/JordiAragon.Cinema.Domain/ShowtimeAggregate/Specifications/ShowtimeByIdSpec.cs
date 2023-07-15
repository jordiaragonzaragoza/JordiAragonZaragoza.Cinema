namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Specifications
{
    using Ardalis.Specification;

    public class ShowtimeByIdSpec : SingleResultSpecification<Showtime>
    {
        public ShowtimeByIdSpec(ShowtimeId showtimeId)
        {
            this.Query
                .Where(showtime => showtime.Id == showtimeId);
        }
    }
}