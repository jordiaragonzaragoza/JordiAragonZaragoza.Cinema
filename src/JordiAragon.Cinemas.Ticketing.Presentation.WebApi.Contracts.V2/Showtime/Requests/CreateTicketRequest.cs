﻿namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests
{
    using System;
    using System.Collections.Generic;

    public record class CreateTicketRequest(Guid ShowtimeId, IEnumerable<Guid> SeatsIds);
}