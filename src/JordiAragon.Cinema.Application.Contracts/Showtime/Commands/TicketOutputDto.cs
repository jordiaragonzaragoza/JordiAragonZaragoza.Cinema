namespace JordiAragon.Cinema.Application.Contracts.Showtime.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Application.Contracts.Auditorium.Queries;

    public record class TicketOutputDto(
        Guid TicketId,
        DateTime SessionDateOnUtc,
        Guid Auditorium,
        string MovieName,
        IEnumerable<SeatOutputDto> Seats);
}