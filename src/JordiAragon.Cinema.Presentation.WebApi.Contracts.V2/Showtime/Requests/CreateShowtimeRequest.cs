﻿namespace JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Requests
{
    using System;

    public record class CreateShowtimeRequest(Guid AuditoriumId, Guid MovieId, DateTime SessionDateOnUtc);
}