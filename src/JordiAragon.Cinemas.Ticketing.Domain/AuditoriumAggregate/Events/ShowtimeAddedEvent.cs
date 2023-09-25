namespace JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeAddedEvent(Guid AuditoriumId, Guid ShowtimeId) : BaseDomainEvent(AuditoriumId);
}