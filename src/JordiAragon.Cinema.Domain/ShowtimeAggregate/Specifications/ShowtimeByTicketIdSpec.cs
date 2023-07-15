namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Specifications
{
    using System.Linq;
    using Ardalis.Specification;

    public class ShowtimeByTicketIdSpec : SingleResultSpecification<Showtime>
    {
        public ShowtimeByTicketIdSpec(TicketId ticketId)
        {
            this.Query
                .Where(showtime => showtime.Tickets.Any(ticket => ticket.Id == ticketId));
        }
    }
}