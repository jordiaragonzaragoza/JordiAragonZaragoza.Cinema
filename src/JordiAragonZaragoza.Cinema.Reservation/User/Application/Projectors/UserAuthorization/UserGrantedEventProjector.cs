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
        private readonly IRepository<UserAuthorizationReadModel, Guid> userAuthorizationReadModelRepository;

        public UserGrantedEventProjector(
            IRepository<UserAuthorizationReadModel, Guid> userAuthorizationReadModelRepository)
        {
            this.userAuthorizationReadModelRepository = userAuthorizationReadModelRepository ?? throw new ArgumentNullException(nameof(userAuthorizationReadModelRepository));
        }

        public override async Task HandleAsync(UserGrantedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var userAuthorizationReadModel = new UserAuthorizationReadModel(
                Guid.NewGuid())
            {
                UserId = @event.AggregateId,
                TenantId = @event.TenantId,
                PartitionId = @event.PartitionId,
                CinemaId = @event.CinemaId,
                Roles = @event.Roles.ToList(),
            };

            await this.userAuthorizationReadModelRepository.AddAsync(userAuthorizationReadModel, cancellationToken);
        }
    }
}