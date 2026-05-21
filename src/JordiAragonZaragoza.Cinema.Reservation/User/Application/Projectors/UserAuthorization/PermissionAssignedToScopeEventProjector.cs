namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.UserAuthorization
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorization;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class PermissionAssignedToScopeEventProjector : BaseEventHandler<PermissionAssignedToScopeEvent>
    {
        private readonly ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository;

        public PermissionAssignedToScopeEventProjector(
            ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public override async Task HandleAsync(PermissionAssignedToScopeEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var query = new GetUserAuthorizationQuery(
                UserId: @event.AggregateId,
                TenantId: @event.TenantId,
                PartitionId: @event.PartitionId,
                CinemaId: @event.CinemaId);

            var readModel = await this.repository.FirstOrDefaultAsync(new GetUserAuthorizationCachedSpecification(query), cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(UserAuthorizationReadModel), $"User {@event.AggregateId} for tenant {@event.TenantId} and partition {@event.PartitionId} and cinema {@event.CinemaId}");
            }

            var permissionsList = (List<PermissionReadModel>)readModel.Permissions;
            permissionsList.Add(new PermissionReadModel(Guid.NewGuid(), @event.Permission));

            await this.repository.UpdateAsync(readModel, cancellationToken);
        }
    }
}