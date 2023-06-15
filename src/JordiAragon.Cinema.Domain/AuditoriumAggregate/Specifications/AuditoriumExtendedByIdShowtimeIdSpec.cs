namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications
{
    using System.Linq;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;

    public class AuditoriumExtendedByIdShowtimeIdSpec : SingleResultSpecification<Auditorium>
    {
        public AuditoriumExtendedByIdShowtimeIdSpec(AuditoriumId id, ShowtimeId showtimeId)
        {
            this.Query
                .Where(auditorium => auditorium.Id == id
                       && auditorium.Showtimes.Any(showtime => showtime == showtimeId));

            this.Query
                .Include(auditorium => auditorium.Showtimes)
                    ////.ThenInclude(showtime => showtime.Tickets)
                        ////.ThenInclude(ticket => ticket.Seats)
                .Include(auditorium => auditorium.Seats);
        }
    }
}