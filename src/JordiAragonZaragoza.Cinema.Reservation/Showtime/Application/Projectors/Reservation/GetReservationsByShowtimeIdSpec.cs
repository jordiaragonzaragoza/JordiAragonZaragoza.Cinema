namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Reservation
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public sealed class GetReservationsByShowtimeIdSpec : Specification<ReservationReadModel>
    {
        public GetReservationsByShowtimeIdSpec(Guid showtimeId)
        {
            this.Query
                .Where(reservationReadModel => reservationReadModel.ShowtimeId == showtimeId)
                .AsNoTracking();
        }
    }
}