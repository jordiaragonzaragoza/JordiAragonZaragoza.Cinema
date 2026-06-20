namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorizations
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;

    /// <summary>
    /// Resolves ALL assignment rows for a user within a tenant — across every scope
    /// level (tenant-only, partition-only, cinema-specific). Used by AuthorizationService
    /// to apply UserAuthorizationReadModel.Matches and select the row whose hierarchy
    /// covers the requested scope, mirroring User.GetRolesFor's in-memory resolution.
    /// </summary>
    public class GetUserAuthorizationsCandidatesCachedSpecification : Specification<UserAuthorizationReadModel>
    {
        public GetUserAuthorizationsCandidatesCachedSpecification(Guid userId, Guid tenantId)
        {
            this.Query
                .Where(u => u.UserId == userId)
                .Where(u => u.TenantId == tenantId)
                .WithCacheKey($"{nameof(GetUserAuthorizationsCandidatesCachedSpecification)}_{userId}_{tenantId}");

            // Note: Intentionally NOT using AsNoTracking() to allow modifications during event projection
        }
    }
}