namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStore
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Infrastructure;
    using JordiAragon.SharedKernel.Infrastructure.EventStore;

    public sealed class ReservationRepository<TAggregate, TId> : BaseRepository<TAggregate, TId>
        where TAggregate : BaseEventSourcedAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public ReservationRepository(IEventStore eventStore)
            : base(eventStore)
        {
        }
    }
}