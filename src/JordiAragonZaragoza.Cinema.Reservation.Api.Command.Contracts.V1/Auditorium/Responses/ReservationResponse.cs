namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Responses
{
    using System;
    using System.Collections.Generic;

    public sealed record class ReservationResponse(
        Guid Id,
        DateTimeOffset SessionDateOnUtc,
        string AuditoriumName,
        string MovieTitle,
        IEnumerable<SeatResponse> Seats,
        bool IsPurchased);
}