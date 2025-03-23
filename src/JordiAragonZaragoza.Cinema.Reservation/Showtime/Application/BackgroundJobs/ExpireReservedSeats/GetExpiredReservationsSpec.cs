namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.BackgroundJobs.ExpireReservedSeats
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public class GetExpiredReservationsSpec : Specification<ReservationReadModel>
    {
        public GetExpiredReservationsSpec(DateTimeOffset currentDateTimeOnUtc)
        {
            this.Query
                .Where(reservation => !reservation.IsPurchased && currentDateTimeOnUtc > reservation.CreatedTimeOnUtc.AddMinutes(1));
        }
    }
}