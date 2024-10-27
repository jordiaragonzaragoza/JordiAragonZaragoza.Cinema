namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class CreateAuditoriumCommand(
        Guid AuditoriumId,
        string Name,
        ushort Rows,
        ushort SeatsPerRow) : ICommand;
}