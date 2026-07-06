namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class CreateAuditoriumCommand(
        Guid AuditoriumId,
        Guid CinemaId,
        string Name,
        ushort Rows,
        ushort SeatsPerRow) : ICommand;
}