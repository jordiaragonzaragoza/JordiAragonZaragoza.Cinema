namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.User
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
        private readonly IRepository<UserReadModel, Guid> userReadModelRepository;

        public UserRemovedEventProjector(
            IRepository<UserReadModel, Guid> userReadModelRepository)
        {
            this.userReadModelRepository = userReadModelRepository ?? throw new ArgumentNullException(nameof(userReadModelRepository));
        }

        public override async Task HandleAsync(UserRemovedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var readModel = await this.userReadModelRepository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(UserReadModel), @event.AggregateId.ToString());
            }

            await this.userReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}