namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class PurchasedReservationEvent(Guid AggregateId, Guid ReservationId) : BaseDomainEvent(AggregateId);
}