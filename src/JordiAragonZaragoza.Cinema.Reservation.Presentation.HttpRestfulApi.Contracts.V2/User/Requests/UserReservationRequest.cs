namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Requests
{
    using System;

    public sealed record class UserReservationRequest(Guid UserId, Guid ShowtimeId, Guid ReservationId)
    {
        public const string Route = "users/{userId}/showtimes/{showtimeId}/reservations/{reservationId}";
    }
}