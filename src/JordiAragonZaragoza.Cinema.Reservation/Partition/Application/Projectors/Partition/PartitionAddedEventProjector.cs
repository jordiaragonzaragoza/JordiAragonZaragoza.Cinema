namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Projectors.Partition
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class PartitionAddedEventProjector : BaseEventHandler<PartitionCreatedEvent>
    {
        private readonly IRepository<PartitionReadModel, Guid> partitionReadModelRepository;

        public PartitionAddedEventProjector(
            IRepository<PartitionReadModel, Guid> partitionReadModelRepository)
        {
            this.partitionReadModelRepository = partitionReadModelRepository ?? throw new ArgumentNullException(nameof(partitionReadModelRepository));
        }

        public override async Task HandleAsync(PartitionCreatedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var partitionReadModel = new PartitionReadModel(
                @event.AggregateId);

            await this.partitionReadModelRepository.AddAsync(partitionReadModel, cancellationToken);
        }
    }
}