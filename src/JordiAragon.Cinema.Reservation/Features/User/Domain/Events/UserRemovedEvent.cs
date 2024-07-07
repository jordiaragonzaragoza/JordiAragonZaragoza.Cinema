namespace JordiAragon.Cinema.Reservation.User.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class UserRemovedEvent(
        Guid AggregateId)
        : BaseDomainEvent(AggregateId);
}