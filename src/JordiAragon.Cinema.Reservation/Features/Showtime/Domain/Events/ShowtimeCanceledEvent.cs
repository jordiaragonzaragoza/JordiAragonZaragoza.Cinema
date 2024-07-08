namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeCanceledEvent(
        Guid AggregateId,
        Guid AuditoriumId,
        Guid MovieId) : BaseDomainEvent(AggregateId);
}