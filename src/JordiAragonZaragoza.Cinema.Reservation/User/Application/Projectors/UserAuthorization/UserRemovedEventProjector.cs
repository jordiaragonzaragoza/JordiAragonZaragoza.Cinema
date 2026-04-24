namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.UserAuthorization
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class UserRemovedEventProjector : BaseEventHandler<UserRemovedEvent>
    {
        private readonly ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository;

        public UserRemovedEventProjector(
            ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public override async Task HandleAsync(UserRemovedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var readModel = await this.repository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(UserAuthorizationReadModel), @event.AggregateId.ToString());
            }

            await this.repository.DeleteAsync(readModel, cancellationToken);
        }
    }
}