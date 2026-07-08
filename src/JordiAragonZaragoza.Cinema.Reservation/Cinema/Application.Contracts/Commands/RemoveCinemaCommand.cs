namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class RemoveCinemaCommand(Guid CinemaId) : ICommand;
}