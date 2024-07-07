namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ReservedSeatsEvent(
        Guid AggregateId,
        Guid TicketId,
        Guid UserId,
        IEnumerable<Guid> SeatIds,
        DateTimeOffset CreatedTimeOnUtc)
        : BaseDomainEvent(AggregateId);
}