namespace JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs.ExpireReservedSeats
{
    using System;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public class GetExpiredTicketsSpec : Specification<TicketReadModel>
    {
        public GetExpiredTicketsSpec(DateTimeOffset currentDateTimeOnUtc)
        {
            this.Query
                .Where(ticket => !ticket.IsPurchased && currentDateTimeOnUtc > ticket.CreatedTimeOnUtc.AddMinutes(1));
        }
    }
}