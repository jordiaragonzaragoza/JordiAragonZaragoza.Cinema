namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class PurchasedTicketEvent(Guid AggregateId, Guid TicketId) : BaseDomainEvent(AggregateId);
}