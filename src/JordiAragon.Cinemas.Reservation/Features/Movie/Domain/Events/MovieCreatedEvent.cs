namespace JordiAragon.Cinemas.Reservation.Movie.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class MovieCreatedEvent(
        Guid MovieId,
        string Title,
        string ImdbId,
        DateTime ReleaseDateOnUtc,
        string Stars)
        : BaseDomainEvent(MovieId);
}