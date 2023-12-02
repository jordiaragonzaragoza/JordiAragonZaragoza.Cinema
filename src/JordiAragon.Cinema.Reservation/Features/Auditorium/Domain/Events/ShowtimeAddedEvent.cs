namespace JordiAragon.Cinema.Reservation.Auditorium.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeAddedEvent(Guid AuditoriumId, Guid ShowtimeId) : BaseDomainEvent(AuditoriumId);
}