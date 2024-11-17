namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class ActiveShowtimeAddedEvent(Guid AggregateId, Guid ShowtimeId) : BaseDomainEvent(AggregateId);
}