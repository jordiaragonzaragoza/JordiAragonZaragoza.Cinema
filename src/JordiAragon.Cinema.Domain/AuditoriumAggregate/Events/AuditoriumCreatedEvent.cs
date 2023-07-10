namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class AuditoriumCreatedEvent(Guid AuditoriumId, IEnumerable<Guid> SeatsIds) : BaseDomainEvent; // TODO: Review if SeatsIds is correct using IEnumerable<Guid> or IEnumerable<Seat>
}