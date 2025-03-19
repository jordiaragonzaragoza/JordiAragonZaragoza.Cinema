namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class ReservedSeatsEvent(
        Guid AggregateId,
        Guid ReservationId,
        Guid UserId,
        IEnumerable<Guid> SeatIds,
        DateTimeOffset CreatedTimeOnUtc)
        : BaseDomainEvent(AggregateId);
}