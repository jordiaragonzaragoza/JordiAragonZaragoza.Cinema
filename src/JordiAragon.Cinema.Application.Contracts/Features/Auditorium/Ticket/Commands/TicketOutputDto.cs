namespace JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Ticket.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Seat.Queries;

    public record class TicketOutputDto(
        Guid TicketId,
        DateTime SessionDateOnUtc,
        Guid Auditorium,
        string MovieName,
        IEnumerable<SeatOutputDto> Seats);
}