namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class TenantCreatedEvent(
        Guid TenantId)
        : BaseDomainEvent(TenantId);
}