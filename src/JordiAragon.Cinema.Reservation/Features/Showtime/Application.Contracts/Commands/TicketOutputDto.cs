namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;

    public record class TicketOutputDto(
        Guid TicketId,
        DateTime SessionDateOnUtc,
        Guid Auditorium,
        string MovieName,
        IEnumerable<SeatOutputDto> Seats,
        bool IsPurchased);
}