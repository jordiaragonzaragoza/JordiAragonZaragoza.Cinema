namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests
{
    using System;

    public sealed record class RevokeUserBodyRequest(
        Guid TenantId,
        Guid? PartitionId,
        Guid? CinemaId);
}