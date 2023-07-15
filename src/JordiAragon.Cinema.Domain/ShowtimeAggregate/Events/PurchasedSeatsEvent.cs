namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class PurchasedSeatsEvent(Guid TicketId) : BaseDomainEvent;
}