namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore
{
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore;

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