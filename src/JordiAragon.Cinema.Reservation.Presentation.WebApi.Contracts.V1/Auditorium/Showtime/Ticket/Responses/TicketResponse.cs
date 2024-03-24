namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Responses
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Responses;

    public record class TicketResponse(
        Guid Id,
        DateTimeOffset SessionDateOnUtc,
        Guid AuditoriumId,
        Guid MovieId,
        IEnumerable<SeatResponse> Seats,
        bool IsPurchased);
}