namespace JordiAragon.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimes
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public class GetShowtimesSpec : Specification<ShowtimeReadModel>
    {
        public GetShowtimesSpec(GetShowtimesQuery request)
        {
            Guard.Against.Null(request);

            // TODO: Complete appropiate query, pagination, etc.
            this.Query
                .Where(showtime => showtime.AuditoriumId == request.AuditoriumId);
        }
    }
}