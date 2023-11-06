namespace JordiAragon.Cinemas.Reservation.Movie.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeAddedEvent(Guid MovieId, Guid ShowtimeId) : BaseDomainEvent(MovieId);
}