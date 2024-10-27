namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class AuditoriumRemovedEvent(Guid AggregateId) : BaseDomainEvent(AggregateId);
}