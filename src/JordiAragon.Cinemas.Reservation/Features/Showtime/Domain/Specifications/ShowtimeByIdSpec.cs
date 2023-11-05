namespace JordiAragon.Cinemas.Reservation.Showtime.Domain.Specifications
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