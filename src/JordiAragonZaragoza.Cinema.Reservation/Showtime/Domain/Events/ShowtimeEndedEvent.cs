namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class ShowtimeEndedEvent(
        Guid AggregateId,
        Guid AuditoriumId,
        Guid MovieId) : BaseDomainEvent(AggregateId);
}