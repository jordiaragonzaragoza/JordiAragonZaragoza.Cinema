namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeCreatedEvent(
        Guid ShowtimeId,
        Guid MovieId,
        DateTime SessionDateOnUtc,
        Guid AuditoriumId)
        : BaseDomainEvent;
}