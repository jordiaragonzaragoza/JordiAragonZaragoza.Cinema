namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class ShowtimeEndedEvent(Guid AggregateId, Guid ShowtimeId) : BaseDomainEvent(AggregateId);
}