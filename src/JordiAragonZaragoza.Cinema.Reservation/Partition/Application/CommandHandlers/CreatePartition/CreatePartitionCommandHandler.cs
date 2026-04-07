namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.CommandHandlers.CreatePartition
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class CreatePartitionCommandHandler : ICommandHandler<CreatePartitionCommand>
    {
        private readonly IRepository<Partition, PartitionId> partitionRepository;

        public CreatePartitionCommandHandler(IRepository<Partition, PartitionId> partitionRepository)
        {
            this.partitionRepository = partitionRepository ?? throw new ArgumentNullException(nameof(partitionRepository));
        }

        public async Task<Result> Handle(CreatePartitionCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var newPartition = Partition.Create(
                id: new PartitionId(request.PartitionId));

            await this.partitionRepository.AddAsync(newPartition, cancellationToken);

            return Result.NoContent();
        }
    }
}