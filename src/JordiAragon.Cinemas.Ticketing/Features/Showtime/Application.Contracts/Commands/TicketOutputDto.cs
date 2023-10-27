namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Application.Contracts.Queries;

    public record class TicketOutputDto(
        Guid TicketId,
        DateTime SessionDateOnUtc,
        Guid Auditorium,
        string MovieName,
        IEnumerable<SeatOutputDto> Seats,
        bool IsPurchased);
}