namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class RemovePermissionCommand(
        Guid UserId,
        Guid TenantId,
        Guid? PartitionId,
        Guid? CinemaId,
        string Permission) : ICommand;
}
