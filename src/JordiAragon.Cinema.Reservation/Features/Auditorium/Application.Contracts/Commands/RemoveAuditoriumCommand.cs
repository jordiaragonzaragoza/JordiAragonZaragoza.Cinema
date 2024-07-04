namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class RemoveAuditoriumCommand(Guid AuditoriumId) : ICommand;
}