namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class PermissionAssignedToScopeEvent(
        Guid AggregateId,
        Guid TenantId,
        Guid? PartitionId,
        Guid? CinemaId,
        string Permission)
        : BaseDomainEvent(AggregateId);
}
