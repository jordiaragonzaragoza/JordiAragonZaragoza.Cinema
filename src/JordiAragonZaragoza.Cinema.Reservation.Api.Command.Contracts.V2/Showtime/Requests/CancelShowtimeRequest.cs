namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests
{
    using System;

    public sealed record class CancelShowtimeRequest(Guid ShowtimeId);
}