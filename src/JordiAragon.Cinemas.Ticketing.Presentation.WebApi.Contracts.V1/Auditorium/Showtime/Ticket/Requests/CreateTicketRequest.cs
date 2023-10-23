namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Requests
{
    using System;
    using System.Collections.Generic;

    public record class CreateTicketRequest(Guid AuditoriumId, Guid ShowtimeId, IEnumerable<Guid> SeatsIds);
}