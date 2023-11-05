namespace JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;

    public record class TicketResponse(
        Guid TicketId,
        DateTime SessionDateOnUtc,
        Guid Auditorium,
        string MovieName,
        IEnumerable<SeatResponse> Seats,
        bool IsPurchased);
}