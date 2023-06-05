namespace JordiAragon.Cinema.Application.Features.Auditorium.Ticket.BackgroundJobs
{
    using System;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;

    public class GetExpiredReserveSeatsSpec : Specification<Ticket>
    {
        public GetExpiredReserveSeatsSpec(DateTime currentDateTimeOnUtc)
        {
            this.Query
                .Where(ticket => !ticket.IsPaid
                                    && currentDateTimeOnUtc > ticket.CreatedTimeOnUtc.AddMinutes(1));
        }
    }
}