namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class ReservationCanceledEvent(
        Guid AggregateId,
        Guid ReservationId,
        bool IsPurchased,
        IEnumerable<Guid> SeatIds)
        : BaseDomainEvent(AggregateId);
}