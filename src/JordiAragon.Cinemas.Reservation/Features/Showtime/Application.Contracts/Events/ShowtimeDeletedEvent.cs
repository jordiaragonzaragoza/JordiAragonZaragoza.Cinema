namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Events
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Events;

    public record class ShowtimeDeletedEvent(Guid ShowtimeId, Guid AuditoriumId, Guid MovieId) : BaseApplicationEvent;
}