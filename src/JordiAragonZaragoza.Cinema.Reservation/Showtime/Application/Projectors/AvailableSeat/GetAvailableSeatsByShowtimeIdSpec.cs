namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.AvailableSeat
{
    using System;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public sealed class GetAvailableSeatsByShowtimeIdSpec : Specification<AvailableSeatReadModel>
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