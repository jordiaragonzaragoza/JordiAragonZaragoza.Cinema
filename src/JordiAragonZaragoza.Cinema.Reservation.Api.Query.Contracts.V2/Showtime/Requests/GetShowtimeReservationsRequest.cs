namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests
{
    using System;

    public sealed record class GetShowtimeReservationsRequest(Guid ShowtimeId);
}