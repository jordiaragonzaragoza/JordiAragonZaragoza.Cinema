namespace JordiAragon.Cinemas.Ticketing.Showtime.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ReservedSeatsEvent(
        Guid ShowtimeId,
        Guid TicketId,
        IEnumerable<Guid> SeatIds,
        DateTime CreatedTimeOnUtc)
        : BaseDomainEvent(ShowtimeId);
}