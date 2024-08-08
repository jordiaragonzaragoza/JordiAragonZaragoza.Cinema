namespace JordiAragon.Cinema.Reservation.Movie.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ActiveShowtimeAddedEvent(Guid AggregateId, Guid ShowtimeId) : BaseDomainEvent(AggregateId);
}