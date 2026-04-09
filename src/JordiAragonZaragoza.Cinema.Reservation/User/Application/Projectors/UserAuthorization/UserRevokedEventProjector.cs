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

    public sealed class UserRevokedEventProjector : BaseEventHandler<UserRevokedEvent>
    {
        private readonly IRepository<UserAuthorizationReadModel, Guid> repository;
        private readonly ISpecificationReadRepository<UserAuthorizationReadModel, Guid> specificationRepository;

        public UserRevokedEventProjector(
            IRepository<UserAuthorizationReadModel, Guid> repository,
            ISpecificationReadRepository<UserAuthorizationReadModel, Guid> specificationRepository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.specificationRepository = specificationRepository ?? throw new ArgumentNullException(nameof(specificationRepository));
        }

        public override async Task HandleAsync(UserRevokedEvent @event, CancellationToken cancellationToken)
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

            await this.repository.DeleteAsync(readModel, cancellationToken);
        }
    }
}