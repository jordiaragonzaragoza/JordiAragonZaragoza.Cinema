namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;

    public record class TicketOutputDto(
        Guid Id,
        DateTimeOffset SessionDateOnUtc,
        Guid AuditoriumId,
        Guid MovieId,
        IEnumerable<SeatOutputDto> Seats,
        bool IsPurchased);
}