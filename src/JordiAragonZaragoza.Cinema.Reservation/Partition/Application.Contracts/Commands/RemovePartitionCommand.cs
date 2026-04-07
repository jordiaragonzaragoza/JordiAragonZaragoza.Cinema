namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class RemovePartitionCommand(Guid PartitionId) : ICommand;
}