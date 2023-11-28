namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories;

    public class ReservationRepository<TAggregate, TId, TIdType> : BaseRepository<TAggregate, TId, TIdType>
        where TAggregate : class, IAggregateRoot<TId, TIdType>
        where TId : class, IEntityId<TIdType>
    {
        public ReservationRepository(ReservationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
