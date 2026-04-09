namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;

    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetUserAuthorizationQuery(Guid UserId, Guid TenantId, Guid? PartitionId, Guid? CinemaId) : IQuery<UserAuthorizationReadModel>;
}