namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class ScheduleShowtimeInAuditoriumCommand(
        Guid AuditoriumId,
        Guid ShowtimeId) : ICommand;
}