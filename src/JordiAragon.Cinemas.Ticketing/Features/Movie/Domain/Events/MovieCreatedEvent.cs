﻿namespace JordiAragon.Cinemas.Ticketing.Movie.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class MovieCreatedEvent(
        Guid MovieId,
        string Title,
        string ImdbId,
        DateTime ReleaseDateOnUtc,
        string Stars)
        : BaseDomainEvent(MovieId);
}