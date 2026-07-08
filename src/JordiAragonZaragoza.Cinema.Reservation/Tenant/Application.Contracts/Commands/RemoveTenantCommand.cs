namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class RemoveTenantCommand(Guid TenantId) : ICommand;
}