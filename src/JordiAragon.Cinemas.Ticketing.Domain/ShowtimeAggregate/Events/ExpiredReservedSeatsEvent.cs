namespace JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ExpiredReservedSeatsEvent(Guid ShowtimeId, Guid TicketId) : BaseDomainEvent(ShowtimeId);
}