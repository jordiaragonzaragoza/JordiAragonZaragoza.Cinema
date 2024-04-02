namespace JordiAragon.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimeTickets
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetShowtimeTicketsSpec : Specification<TicketReadModel>, IPaginatedSpecification<TicketReadModel>
    {
        private readonly GetShowtimeTicketsQuery request;

        public GetShowtimeTicketsSpec(GetShowtimeTicketsQuery request)
        {
            this.request = Guard.Against.Null(request);

            this.Query
                .Where(ticket => ticket.ShowtimeId == request.ShowtimeId)
                .Include(ticket => ticket.Seats)
                .AsNoTracking();
        }

        public IPaginatedQuery Request
            => this.request;
    }
}