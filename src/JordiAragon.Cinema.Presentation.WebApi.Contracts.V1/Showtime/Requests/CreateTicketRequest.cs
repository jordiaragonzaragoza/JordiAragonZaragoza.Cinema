namespace JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Showtime.Requests
{
    using System;
    using System.Collections.Generic;

    public record class CreateTicketRequest(IEnumerable<Guid> SeatsIds);
}