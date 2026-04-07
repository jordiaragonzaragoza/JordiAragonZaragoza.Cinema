namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.CommandHandlers.RemovePartition
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class RemovePartitionCommandHandler : ICommandHandler<RemovePartitionCommand>
    {
        private readonly IRepository<Partition, PartitionId> partitionRepository;

        public RemovePartitionCommandHandler(IRepository<Partition, PartitionId> partitionRepository)
        {
            this.partitionRepository = partitionRepository ?? throw new ArgumentNullException(nameof(partitionRepository));
        }

        public async Task<Result> Handle(RemovePartitionCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingPartition = await this.partitionRepository.GetByIdAsync(new PartitionId(request.PartitionId), cancellationToken);
            if (existingPartition is null)
            {
                return Result.NotFound($"{nameof(Partition)}: {request.PartitionId} not found.");
            }

            // TODO: Before remove Partition check if there is some scheduled showtime regarding to Partition via domain service.
            existingPartition.Remove();

            await this.partitionRepository.DeleteAsync(existingPartition, cancellationToken);

            return Result.NoContent();
        }
    }
}