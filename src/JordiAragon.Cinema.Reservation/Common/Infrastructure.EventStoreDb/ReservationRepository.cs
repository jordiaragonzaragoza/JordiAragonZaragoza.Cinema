namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStoreDb
{
    using global::EventStore.Client;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Infrastructure.EventStore;
    using Microsoft.Extensions.Logging;

    public class ReservationRepository<TAggregate, TId> : BaseRepository<TAggregate, TId>
        where TAggregate : BaseEventSourcedAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public ReservationRepository(
            EventStoreClient eventStoreClient,
            ILogger<ReservationRepository<TAggregate, TId>> logger)
            : base(eventStoreClient, logger)
        {
        }
    }
}
