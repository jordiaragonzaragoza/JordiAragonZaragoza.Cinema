namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Ticket
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public sealed class GetTicketsByShowtimeIdSpec : Specification<TicketReadModel>
    {
        public GetTicketsByShowtimeIdSpec(Guid showtimeId)
        {
            this.Query
                .Where(ticket => ticket.ShowtimeId == showtimeId)
                .AsNoTracking();
        }
    }
}