namespace JordiAragon.Cinemas.Ticketing.Domain.MovieAggregate.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeAddedEvent(Guid MovieId, Guid ShowtimeId) : BaseDomainEvent(MovieId);
}