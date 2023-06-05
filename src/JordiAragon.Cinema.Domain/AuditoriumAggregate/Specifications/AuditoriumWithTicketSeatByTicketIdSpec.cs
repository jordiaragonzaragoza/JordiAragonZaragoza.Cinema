namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications
{
    using System.Linq;
    using Ardalis.Specification;

    public class AuditoriumWithTicketSeatByTicketIdSpec : SingleResultSpecification<Auditorium>
    {
        public AuditoriumWithTicketSeatByTicketIdSpec(TicketId ticketId)
        {
            this.Query
                .Where(auditorium => auditorium.Showtimes.Any(showtime => showtime.Tickets.Any(ticket => ticket.Id == ticketId)));

            this.Query
                .Include(auditorium => auditorium.Showtimes)
                    .ThenInclude(showtime => showtime.Tickets)
                        .ThenInclude(ticket => ticket.Seats);
        }
    }
}