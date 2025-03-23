namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimeReservations
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetShowtimeReservationsSpec : Specification<ReservationReadModel>, IPaginatedSpecification<ReservationReadModel>
    {
        private readonly GetShowtimeReservationsQuery request;

        public GetShowtimeReservationsSpec(GetShowtimeReservationsQuery request)
        {
            this.request = Guard.Against.Null(request);

            this.Query
                .Where(reservation => reservation.ShowtimeId == request.ShowtimeId)
                .Include(reservation => reservation.Seats)
                .AsNoTracking();
        }

        public IPaginatedQuery Request
            => this.request;
    }
}