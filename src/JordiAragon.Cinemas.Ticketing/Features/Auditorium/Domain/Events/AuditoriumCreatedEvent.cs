namespace JordiAragon.Cinemas.Ticketing.Auditorium.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class AuditoriumCreatedEvent(Guid AuditoriumId, short Rows, short SeatsPerRow) : BaseDomainEvent(AuditoriumId);
}