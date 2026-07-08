namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GrantUserCommand(
        Guid UserId,
        Guid TenantId,
        Guid? PartitionId,
        Guid? CinemaId,
        IEnumerable<string>? Roles,
        IEnumerable<string>? Permissions) : ICommand;
}