namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.UserAuthorization
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class UserGrantedEventProjector : BaseEventHandler<UserGrantedEvent>
    {
        private readonly ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository;

        public UserGrantedEventProjector(
            ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public override async Task HandleAsync(UserGrantedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var userAuthorizationReadModel = new UserAuthorizationReadModel(
                Guid.CreateVersion7())
            {
                UserId = @event.AggregateId,
                TenantId = @event.TenantId,
                PartitionId = @event.PartitionId,
                DomainId = @event.CinemaId,
            };

            foreach (var role in @event.Roles)
            {
                userAuthorizationReadModel.Roles.Add(
                    new RoleReadModel(Guid.CreateVersion7(), role));
            }

            foreach (var permission in @event.Permissions)
            {
                userAuthorizationReadModel.Permissions.Add(
                    new PermissionReadModel(Guid.CreateVersion7(), permission));
            }

            await this.repository.AddAsync(userAuthorizationReadModel, cancellationToken);
        }
    }
}