﻿namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests
{
    using System;

    public sealed record class GetAvailableSeatsRequest(Guid ShowtimeId);
}