namespace JordiAragon.Cinemas.Reservation.Showtime.Domain.Specifications
{
    using System.Linq;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;

    public class ShowtimeByTicketIdSpec : SingleResultSpecification<Showtime>
    {
        public ShowtimeByTicketIdSpec(TicketId ticketId)
        {
            Guard.Against.Null(ticketId);

            this.Query
                .Where(showtime => showtime.Tickets.Any(ticket => ticket.Id == ticketId));
        }
    }
}