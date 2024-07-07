namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class CreateAuditoriumCommand(
        Guid AuditoriumId,
        string Name,
        ushort Rows,
        ushort SeatsPerRow) : ICommand;
}