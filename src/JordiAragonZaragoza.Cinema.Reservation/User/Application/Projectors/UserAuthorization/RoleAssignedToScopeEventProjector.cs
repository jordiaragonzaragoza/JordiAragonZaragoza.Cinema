namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.UserAuthorization
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorizations;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class RoleAssignedToScopeEventProjector : BaseEventHandler<RoleAssignedToScopeEvent>
    {
        private readonly ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository;

        public RoleAssignedToScopeEventProjector(
            ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public override async Task HandleAsync(RoleAssignedToScopeEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var query = new GetUserAuthorizationsQuery(
                UserId: @event.AggregateId,
                TenantId: @event.TenantId,
                PartitionId: @event.PartitionId,
                CinemaId: @event.CinemaId);

            var readModel = await this.repository.FirstOrDefaultAsync(new GetUserAuthorizationCachedSpecification(query), cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(UserAuthorizationReadModel), $"User {@event.AggregateId} for tenant {@event.TenantId} and partition {@event.PartitionId} and cinema {@event.CinemaId}");
            }

            var rolesList = (List<RoleReadModel>)readModel.Roles;
            rolesList.Add(new RoleReadModel(Guid.CreateVersion7(), @event.Role));

            await this.repository.UpdateAsync(readModel, cancellationToken);
        }
    }
}