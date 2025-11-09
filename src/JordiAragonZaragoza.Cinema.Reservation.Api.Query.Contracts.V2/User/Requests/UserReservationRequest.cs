namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests
{
    using System;

    public sealed record class UserReservationRequest(Guid UserId, Guid ShowtimeId, Guid ReservationId);
}