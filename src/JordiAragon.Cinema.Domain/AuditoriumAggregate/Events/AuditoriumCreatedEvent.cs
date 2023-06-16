namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Events
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class AuditoriumCreatedEvent(AuditoriumId AuditoriumId, IEnumerable<Seat> Seats) : BaseDomainEvent;
}