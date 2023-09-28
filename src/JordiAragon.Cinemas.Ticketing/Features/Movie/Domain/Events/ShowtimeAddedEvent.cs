namespace JordiAragon.Cinemas.Ticketing.Movie.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeAddedEvent(Guid MovieId, Guid ShowtimeId) : BaseDomainEvent(MovieId);
}