namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ReservedSeatsEvent(
        Guid ShowtimeId,
        Guid TicketId,
        Guid UserId,
        IEnumerable<Guid> SeatIds,
        DateTimeOffset CreatedTimeOnUtc)
        : BaseDomainEvent(ShowtimeId);
}