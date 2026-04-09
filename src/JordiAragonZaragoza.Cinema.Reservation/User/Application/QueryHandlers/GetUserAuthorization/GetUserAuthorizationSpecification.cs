namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorization
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;

    public sealed class GetUserAuthorizationSpecification : SingleResultSpecification<UserAuthorizationReadModel>
    {
        public GetUserAuthorizationSpecification(GetUserAuthorizationQuery request)
        {
            ArgumentNullException.ThrowIfNull(request);

            this.Query
                .Where(userAuthorization => userAuthorization.UserId == request.UserId)
                .Where(userAuthorization => userAuthorization.TenantId == request.TenantId)
                .Where(userAuthorization => userAuthorization.PartitionId == request.PartitionId, request.PartitionId is not null)
                .Where(userAuthorization => userAuthorization.CinemaId == request.CinemaId, request.CinemaId is not null)
                .AsNoTracking();
        }
    }
}