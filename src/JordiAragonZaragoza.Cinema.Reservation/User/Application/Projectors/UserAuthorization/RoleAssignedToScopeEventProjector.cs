namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.UserAuthorization
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorization;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class RoleAssignedToScopeEventProjector : BaseEventHandler<RoleAssignedToScopeEvent>
    {
        private readonly ISpecificationReadRepository<UserAuthorizationReadModel, Guid> specificationRepository;
        private readonly IRepository<UserAuthorizationReadModel, Guid> repository;

        public RoleAssignedToScopeEventProjector(
            ISpecificationReadRepository<UserAuthorizationReadModel, Guid> specificationRepository,
            IRepository<UserAuthorizationReadModel, Guid> repository)
        {
            this.specificationRepository = specificationRepository ?? throw new ArgumentNullException(nameof(specificationRepository));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public override async Task HandleAsync(RoleAssignedToScopeEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var query = new GetUserAuthorizationQuery(
                UserId: @event.AggregateId,
                TenantId: @event.TenantId,
                PartitionId: @event.PartitionId,
                CinemaId: @event.CinemaId);

            var readModel = await this.specificationRepository.FirstOrDefaultAsync(new GetUserAuthorizationSpecification(query), cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(UserAuthorizationReadModel), $"User {@event.AggregateId} for tenant {@event.TenantId} and partition {@event.PartitionId} and cinema {@event.CinemaId}");
            }

            readModel.Roles.Add(@event.Role);

            await this.repository.UpdateAsync(readModel, cancellationToken);
        }
    }
}