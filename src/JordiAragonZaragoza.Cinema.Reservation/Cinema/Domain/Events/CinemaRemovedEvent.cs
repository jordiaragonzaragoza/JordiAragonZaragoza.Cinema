namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class CinemaRemovedEvent(
        Guid AggregateId)
        : BaseDomainEvent(AggregateId);
}