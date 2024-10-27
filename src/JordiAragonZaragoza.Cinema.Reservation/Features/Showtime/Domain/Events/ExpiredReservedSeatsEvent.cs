namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class ExpiredReservedSeatsEvent(Guid AggregateId, Guid TicketId) : BaseDomainEvent(AggregateId);
}