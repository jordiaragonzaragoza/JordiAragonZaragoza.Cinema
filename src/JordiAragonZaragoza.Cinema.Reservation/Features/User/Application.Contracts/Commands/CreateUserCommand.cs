namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class CreateUserCommand(Guid UserId) : ICommand;
}