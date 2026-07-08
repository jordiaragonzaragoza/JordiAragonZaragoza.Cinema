namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class TenantRemovedEvent(
        Guid AggregateId)
        : BaseDomainEvent(AggregateId);
}