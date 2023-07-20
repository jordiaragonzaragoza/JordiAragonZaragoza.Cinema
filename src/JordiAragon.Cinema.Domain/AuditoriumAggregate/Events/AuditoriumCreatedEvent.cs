namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class AuditoriumCreatedEvent(Guid AuditoriumId, short Rows, short SeatsPerRow) : BaseDomainEvent(AuditoriumId);
}