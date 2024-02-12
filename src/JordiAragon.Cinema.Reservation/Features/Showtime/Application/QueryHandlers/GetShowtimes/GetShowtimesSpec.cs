namespace JordiAragon.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimes
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public class GetShowtimesSpec : Specification<ShowtimeReadModel>, IPaginatedSpecification<ShowtimeReadModel>
    {
        private readonly GetShowtimesQuery request;

        public GetShowtimesSpec(GetShowtimesQuery request)
        {
            this.request = Guard.Against.Null(request);

            this.Query
                .Where(s => s.AuditoriumId == request.AuditoriumId, request.AuditoriumId is not null)
                .AsNoTracking()
                .OrderByDescending(x => x.SessionDateOnUtc);
        }

        public IPaginatedQuery Request
            => this.request;
    }
}