namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorization
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;

    public sealed class GetUserAuthorizationSpecification : SingleResultSpecification<UserAuthorizationReadModel>
    {
        public GetUserAuthorizationSpecification(GetUserAuthorizationQuery query)
        {
            ArgumentNullException.ThrowIfNull(query);

            this.Query
                .Where(userAuthorization => userAuthorization.UserId == query.UserId)
                .Where(userAuthorization => userAuthorization.TenantId == query.TenantId)
                .Where(userAuthorization => userAuthorization.PartitionId == query.PartitionId, query.PartitionId is not null)
                .Where(userAuthorization => userAuthorization.CinemaId == query.CinemaId, query.CinemaId is not null);

            // Note: Intentionally NOT using AsNoTracking() to allow modifications during event projection
        }
    }
}