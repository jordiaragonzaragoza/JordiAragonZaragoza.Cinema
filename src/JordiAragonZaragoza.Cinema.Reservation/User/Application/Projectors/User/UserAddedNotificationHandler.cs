namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.User
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class UserAddedNotificationHandler : BaseEventHandler<UserCreatedEvent>
    {
        private readonly IRepository<UserReadModel, Guid> userReadModelRepository;

        public UserAddedNotificationHandler(
            IRepository<UserReadModel, Guid> userReadModelRepository)
        {
            this.userReadModelRepository = userReadModelRepository ?? throw new ArgumentNullException(nameof(userReadModelRepository));
        }

        public override async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var userReadModel = new UserReadModel(
                @event.AggregateId);

            await this.userReadModelRepository.AddAsync(userReadModel, cancellationToken);
        }
    }
}