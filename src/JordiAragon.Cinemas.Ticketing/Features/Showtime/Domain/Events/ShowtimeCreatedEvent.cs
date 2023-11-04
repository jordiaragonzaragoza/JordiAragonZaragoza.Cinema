namespace JordiAragon.Cinemas.Ticketing.Showtime.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeCreatedEvent(
        Guid ShowtimeId,
        Guid MovieId,
        DateTime SessionDateOnUtc,
        Guid AuditoriumId)
        : BaseDomainEvent(ShowtimeId);
}