namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class PurchasedTicketEvent(Guid AggregateId, Guid TicketId) : BaseDomainEvent(AggregateId);
}