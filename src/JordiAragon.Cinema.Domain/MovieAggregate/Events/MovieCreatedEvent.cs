namespace JordiAragon.Cinema.Domain.MovieAggregate.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class MovieCreatedEvent(
        MovieId MovieId,
        string Title,
        string ImdbId,
        DateTime ReleaseDateOnUtc,
        string Stars)
        : BaseDomainEvent;
}