namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class ActiveShowtimeRemovedEvent(Guid AggregateId, Guid ShowtimeId) : BaseDomainEvent(AggregateId);
}