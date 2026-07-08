namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.QueryHandlers.GetPartitions
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetPartitionsSpec : Specification<PartitionReadModel>, IPaginatedSpecification<PartitionReadModel>
    {
        private readonly GetPartitionsQuery request;

        public GetPartitionsSpec(GetPartitionsQuery request)
        {
            this.request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public IPaginatedQuery Request
            => this.request;
    }
}