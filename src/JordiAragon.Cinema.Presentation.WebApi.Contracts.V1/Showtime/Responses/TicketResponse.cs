namespace JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Showtime.Responses
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Auditorium.Responses;

    public record class TicketResponse(Guid TicketId, DateTime SessionDateOnUtc, Guid Auditorium, string MovieName, IEnumerable<SeatResponse> Seats);
}