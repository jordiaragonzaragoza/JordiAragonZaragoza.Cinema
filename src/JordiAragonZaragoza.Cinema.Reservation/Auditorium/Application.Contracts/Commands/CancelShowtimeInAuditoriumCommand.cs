namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class CancelShowtimeInAuditoriumCommand(
        Guid AuditoriumId,
        Guid ShowtimeId) : ICommand;
}