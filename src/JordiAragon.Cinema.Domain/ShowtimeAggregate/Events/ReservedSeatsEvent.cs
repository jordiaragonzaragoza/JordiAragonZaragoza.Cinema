namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ReservedSeatsEvent(TicketId TicketId, IEnumerable<SeatId> SeatIds, DateTime CreatedTimeOnUtc) : BaseDomainEvent;
}