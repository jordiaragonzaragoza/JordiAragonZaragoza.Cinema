namespace JordiAragon.Cinema.Reservation.Auditorium.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class AuditoriumCreatedEvent(Guid AuditoriumId, string Name, ushort Rows, ushort SeatsPerRow) : BaseDomainEvent(AuditoriumId);
}