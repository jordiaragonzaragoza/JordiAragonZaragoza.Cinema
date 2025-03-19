namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;

    public static class CreateReservationUtils
    {
        public static Reservation Create()
            => new Reservation(
                Constants.Reservation.Id,
                Constants.Reservation.UserId,
                Constants.Reservation.SeatIds,
                Constants.Reservation.ReservationDateOnUtc);
    }
}