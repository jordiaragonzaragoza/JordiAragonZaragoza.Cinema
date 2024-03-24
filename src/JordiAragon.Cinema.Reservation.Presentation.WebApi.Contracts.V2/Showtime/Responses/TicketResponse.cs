namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;

    public record class TicketResponse(
        Guid Id,
        DateTimeOffset SessionDateOnUtc,
        Guid AuditoriumId,
        Guid MovieId,
        IEnumerable<SeatResponse> Seats,
        bool IsPurchased);
}