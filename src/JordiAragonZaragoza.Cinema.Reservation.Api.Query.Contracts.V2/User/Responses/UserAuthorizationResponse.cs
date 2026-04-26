namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Responses
{
    using System;
    using System.Collections.Generic;

    public sealed record class UserAuthorizationResponse(
        Guid Id,
        Guid UserId,
        Guid TenantId,
        Guid? PartitionId,
        Guid? CinemaId,
        IEnumerable<RoleResponse> Roles);
}
