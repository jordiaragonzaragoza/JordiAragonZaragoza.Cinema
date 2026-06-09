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
                CinemaId = @event.CinemaId,
                Roles = @event.Roles
                    .Select(r => new RoleReadModel(Guid.CreateVersion7(), r))
                    .ToList(),
                Permissions = @event.Permissions
                    .Select(p => new PermissionReadModel(Guid.CreateVersion7(), p))
                    .ToList(),
            };

            await this.repository.AddAsync(userAuthorizationReadModel, cancellationToken);
        }
    }
}