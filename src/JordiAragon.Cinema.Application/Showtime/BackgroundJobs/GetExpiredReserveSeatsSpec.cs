namespace JordiAragon.Cinema.Application.Showtime.BackgroundJobs
{
    using System;
    using System.Linq;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;

    public class GetExpiredReserveSeatsSpec : Specification<Showtime>
    {
        public GetExpiredReserveSeatsSpec(DateTime currentDateTimeOnUtc)
        {
            this.Query
                .Where(showtime => currentDateTimeOnUtc < showtime.SessionDateOnUtc
                                    && showtime.Tickets.Any(ticket => !ticket.IsPaid && currentDateTimeOnUtc > ticket.CreatedTimeOnUtc.AddMinutes(1)));

            this.Query.Include(showtime => showtime.Tickets);
        }
    }
}