namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class AuditoriumCreatedEvent(Guid AggregateId, string Name, ushort Rows, ushort SeatsPerRow) : BaseDomainEvent(AggregateId);
}