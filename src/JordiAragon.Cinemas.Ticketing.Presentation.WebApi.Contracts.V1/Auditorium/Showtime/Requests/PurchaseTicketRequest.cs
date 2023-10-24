﻿namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests
{
    using System;

    public record class PurchaseTicketRequest(Guid AuditoriumId, Guid ShowtimeId, Guid TicketId);
}