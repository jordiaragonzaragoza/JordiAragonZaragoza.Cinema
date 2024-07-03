namespace JordiAragon.Cinema.Reservation.Movie.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class MovieRemovedEvent(
        Guid AggregateId)
        : BaseDomainEvent(AggregateId);
}