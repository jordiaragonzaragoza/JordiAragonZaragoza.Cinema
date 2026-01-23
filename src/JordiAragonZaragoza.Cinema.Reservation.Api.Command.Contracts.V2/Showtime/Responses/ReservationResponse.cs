namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses
{
    using System;
    using System.Collections.Generic;

    public sealed record class ReservationResponse(
        Guid Id,
        Guid UserId,
        Guid ShowtimeId,
        DateTimeOffset SessionDateOnUtc,
        string AuditoriumName,
        string MovieTitle,
        IEnumerable<SeatResponse> Seats,
        bool IsPurchased);
}