namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class ExpiredReservedSeatsEvent(
        Guid AggregateId,
        Guid TicketId,
        IEnumerable<Guid> SeatIds)
        : BaseDomainEvent(AggregateId);
}