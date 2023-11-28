namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories;

    public class ReservationReadRepository<TEntity, TId, TIdType> : BaseReadRepository<TEntity, TId, TIdType>
        where TEntity : class, IEntity<TId, TIdType>
        where TId : class, IEntityId<TIdType>
    {
        public ReservationReadRepository(ReservationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
