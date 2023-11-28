namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories;

    public class ReservationSpecificationReadRepository<TEntity, TId, TIdType> : BaseSpecificationReadRepository<TEntity, TId, TIdType>
        where TEntity : class, IEntity<TId, TIdType>
        where TId : class, IEntityId<TIdType>
    {
        public ReservationSpecificationReadRepository(ReservationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
