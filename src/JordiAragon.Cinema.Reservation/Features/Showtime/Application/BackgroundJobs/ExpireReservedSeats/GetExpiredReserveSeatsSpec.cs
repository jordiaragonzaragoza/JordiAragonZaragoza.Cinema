namespace JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs.ExpireReservedSeats
{
    using System;
    using System.Linq;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;

    public class GetExpiredReserveSeatsSpec : Specification<Showtime>
    {
        public GetExpiredReserveSeatsSpec(DateTimeOffset currentDateTimeOnUtc)
        {
            this.Query
                .Where(showtime => currentDateTimeOnUtc < showtime.SessionDateOnUtc // TODO: Review. Not SessionDate. SessionEndDateTime.
                                    && showtime.Tickets.Any(ticket => !ticket.IsPurchased && currentDateTimeOnUtc > ticket.CreatedTimeOnUtc.AddMinutes(1)));

            this.Query.Include(showtime => showtime.Tickets);
        }
    }
}