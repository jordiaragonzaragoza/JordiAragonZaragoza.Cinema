namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ReservedSeatsEvent(Guid TicketId, IEnumerable<Guid> SeatIds, DateTime CreatedTimeOnUtc) : BaseDomainEvent;
}