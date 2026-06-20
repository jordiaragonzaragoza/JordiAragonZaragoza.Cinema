namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorizations
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;

    /// <summary>
    /// Resolves the EXACT scope-level row for a given (userId, tenantId, partitionId, cinemaId).
    /// Used by event projectors to locate the precise assignment row that a domain
    /// event refers to. Does NOT apply scope hierarchy — an event with a specific
    /// scope must only ever mutate the row matching that exact scope.
    /// </summary>
    public sealed class GetUserAuthorizationCachedSpecification : SingleResultSpecification<UserAuthorizationReadModel>
    {
        public GetUserAuthorizationCachedSpecification(GetUserAuthorizationsQuery query)
        {
            ArgumentNullException.ThrowIfNull(query);

            this.Query
                .Where(userAuthorization => userAuthorization.UserId == query.UserId)
                .Where(userAuthorization => userAuthorization.TenantId == query.TenantId)
                .Where(userAuthorization => userAuthorization.PartitionId == query.PartitionId, query.PartitionId is not null)
                .Where(userAuthorization => userAuthorization.CinemaId == query.CinemaId, query.CinemaId is not null)
                .WithCacheKey($"{typeof(GetUserAuthorizationCachedSpecification).Name}_{query.UserId}_{query.TenantId}_{query.PartitionId}_{query.CinemaId}");

            // Note: Intentionally NOT using AsNoTracking() to allow modifications during event projection
        }
    }
}