namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class UserGrantedEvent(
        Guid AggregateId,
        Guid TenantId,
        Guid? PartitionId,
        Guid? CinemaId,
        IEnumerable<string> Roles)
        : BaseDomainEvent(AggregateId);
}