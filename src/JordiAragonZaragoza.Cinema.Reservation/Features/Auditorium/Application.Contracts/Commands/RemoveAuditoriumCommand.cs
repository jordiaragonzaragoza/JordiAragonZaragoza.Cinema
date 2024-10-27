namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class RemoveAuditoriumCommand(Guid AuditoriumId) : ICommand;
}