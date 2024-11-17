namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetAvailableSeats
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public sealed class GetAvailableSeatsByShowtimeIdSpec : Specification<AvailableSeatReadModel>
    {
        public GetAvailableSeatsByShowtimeIdSpec(Guid showtimeId)
        {
            this.Query
                .Where(seat => seat.ShowtimeId == showtimeId)
                .AsNoTracking();
        }
    }
}