namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events
{
    using System;
    using System.Collections.ObjectModel;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class AuditoriumCreatedEvent(
        Guid AggregateId,
        string Name,
        ushort Rows,
        ushort SeatsPerRow,
        ReadOnlyCollection<Guid> SeatIds,
        ReadOnlyCollection<ushort> SeatRows,
        ReadOnlyCollection<ushort> SeatNumbers) : BaseDomainEvent(AggregateId);
}