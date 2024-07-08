namespace JordiAragon.Cinema.Reservation.User.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class CreateUserCommand(Guid UserId) : ICommand;
}