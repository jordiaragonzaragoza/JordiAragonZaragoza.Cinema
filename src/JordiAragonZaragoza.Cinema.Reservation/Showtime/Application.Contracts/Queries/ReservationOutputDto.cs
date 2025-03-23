namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;

    public sealed record class ReservationOutputDto(
        Guid Id,
        Guid UserId,
        Guid ShowtimeId,
        DateTimeOffset SessionDateOnUtc,
        string AuditoriumName,
        string MovieTitle,
        IEnumerable<SeatOutputDto> Seats,
        bool IsPurchased);
}