namespace JordiAragon.Cinema.Reservation.User.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class UserCreatedEvent(
        Guid UserId)
        : BaseDomainEvent(UserId);
}