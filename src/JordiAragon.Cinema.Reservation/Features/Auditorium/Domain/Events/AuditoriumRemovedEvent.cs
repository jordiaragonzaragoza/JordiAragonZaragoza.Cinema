namespace JordiAragon.Cinema.Reservation.Auditorium.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class AuditoriumRemovedEvent(Guid AggregateId) : BaseDomainEvent(AggregateId);
}