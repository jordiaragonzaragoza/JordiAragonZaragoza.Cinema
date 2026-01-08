namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class AddActiveShowtimeCommand(
        Guid AuditoriumId,
        Guid ShowtimeId) : ICommand;
}