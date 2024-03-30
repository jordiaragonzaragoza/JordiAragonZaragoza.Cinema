namespace JordiAragon.Cinema.Reservation.Showtime.Application.QueryHandlers.GetAvailableSeats
{
    using System;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public class GetAvailableSeatsByShowtimeIdSpec : Specification<AvailableSeatReadModel>
    {
        public GetAvailableSeatsByShowtimeIdSpec(Guid showtimeId)
        {
            this.Query
                .Where(seat => seat.ShowtimeId == showtimeId)
                .AsNoTracking();
        }
    }
}