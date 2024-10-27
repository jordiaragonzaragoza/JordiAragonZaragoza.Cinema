namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserTickets
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetUserTicketsSpecification : Specification<TicketReadModel>, IPaginatedSpecification<TicketReadModel>
    {
        private readonly GetUserTicketsQuery request;

        public GetUserTicketsSpecification(GetUserTicketsQuery request)
        {
            this.request = Guard.Against.Null(request);

            this.Query
                .Where(t => t.UserId == request.UserId)
                .Where(t => t.ShowtimeId == request.ShowtimeId, request.ShowtimeId is not null)
                .Where(t => t.AuditoriumName.Contains(request.AuditoriumName), !string.IsNullOrWhiteSpace(request.AuditoriumName))
                .Where(t => t.MovieTitle.Contains(request.MovieTitle), !string.IsNullOrWhiteSpace(request.MovieTitle))
                .Where(s => s.SessionDateOnUtc >= request.StartIntervalTimeOnUtc, request.StartIntervalTimeOnUtc is not null)
                .Where(s => s.SessionDateOnUtc <= request.EndIntervalTimeOnUtc, request.EndIntervalTimeOnUtc is not null)
                .AsNoTracking()
                .OrderByDescending(t => t.SessionDateOnUtc);
        }

        public IPaginatedQuery Request
            => this.request;
    }
}