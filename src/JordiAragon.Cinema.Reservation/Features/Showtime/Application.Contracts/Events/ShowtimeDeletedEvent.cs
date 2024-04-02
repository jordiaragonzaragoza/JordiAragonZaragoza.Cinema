namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Events;

    public sealed record class ShowtimeDeletedEvent(Guid ShowtimeId, Guid AuditoriumId, Guid MovieId) : BaseApplicationEvent;
}