namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests
{
    using System;

    public sealed record class UserAuthorizationRequest(
        Guid UserId,
        Guid TenantId,
        Guid? PartitionId,
        Guid? CinemaId);
}
