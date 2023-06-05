namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications
{
    using System.Linq;
    using Ardalis.Specification;

    public class AuditoriumWithReservedSeatsByIdShowtimeIdTicketIdSpec : SingleResultSpecification<Auditorium>
    {
        public AuditoriumWithReservedSeatsByIdShowtimeIdTicketIdSpec(AuditoriumId id, ShowtimeId showtimeId, TicketId ticketId)
        {
            this.Query
                .Where(auditorium => auditorium.Id == id
                    && auditorium.Showtimes.Any(showtime => showtime.Id == showtimeId
                                                         && showtime.Tickets.Any(ticket => ticket.Id == ticketId)));

            this.Query
                .Include(auditorium => auditorium.Showtimes)
                    .ThenInclude(showtime => showtime.Tickets)
                        .ThenInclude(ticket => ticket.Seats);
        }
    }
}