namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed record class GrantUserBodyRequest(
        Guid TenantId,
        Guid? PartitionId,
        Guid? CinemaId,
        IEnumerable<string>? Roles,
        IEnumerable<string>? Permissions);
}