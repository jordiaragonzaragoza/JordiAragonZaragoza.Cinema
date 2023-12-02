namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories;

    public class ReservationSpecificationReadRepository<TEntity, TId> : BaseSpecificationReadRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : class, IEntityId
    {
        public ReservationSpecificationReadRepository(ReservationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
