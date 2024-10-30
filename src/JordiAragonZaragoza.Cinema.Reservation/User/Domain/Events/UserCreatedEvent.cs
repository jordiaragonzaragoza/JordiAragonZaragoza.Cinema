namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class UserCreatedEvent(
        Guid UserId)
        : BaseDomainEvent(UserId);
}