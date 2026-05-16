namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorization
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;

    public sealed class GetUserAuthorizationCachedSpecification : SingleResultSpecification<UserAuthorizationReadModel>
    {
        public GetUserAuthorizationCachedSpecification(GetUserAuthorizationQuery query)
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