namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Projectors.Partition
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class PartitionRemovedEventProjector : BaseEventHandler<PartitionRemovedEvent>
    {
        private readonly IRepository<PartitionReadModel, Guid> partitionReadModelRepository;

        public PartitionRemovedEventProjector(
            IRepository<PartitionReadModel, Guid> partitionReadModelRepository)
        {
            this.partitionReadModelRepository = partitionReadModelRepository ?? throw new ArgumentNullException(nameof(partitionReadModelRepository));
        }

        public override async Task HandleAsync(PartitionRemovedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var readModel = await this.partitionReadModelRepository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(PartitionReadModel), @event.AggregateId.ToString());
            }

            await this.partitionReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}