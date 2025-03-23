﻿namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Reservation.Responses
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Responses;

    public sealed record class ReservationResponse(
        Guid Id,
        DateTimeOffset SessionDateOnUtc,
        string AuditoriumName,
        string MovieTitle,
        IEnumerable<SeatResponse> Seats,
        bool IsPurchased);
}