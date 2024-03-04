namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projections.AvailableSeat
{
    using System;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public class GetAvailableSeatsByShowtimeIdSpec : Specification<AvailableSeatReadModel>
    {
        public GetAvailableSeatsByShowtimeIdSpec(Guid showtimeId)
        {
            Guard.Against.Default(showtimeId);

            this.Query
                .Where(av => av.ShowtimeId == showtimeId)
                .AsNoTracking();
        }
    }
}