namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories;

    public class ReservationReadRepository<TEntity, TId> : BaseReadRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : class, IEntityId
    {
        public ReservationReadRepository(ReservationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
