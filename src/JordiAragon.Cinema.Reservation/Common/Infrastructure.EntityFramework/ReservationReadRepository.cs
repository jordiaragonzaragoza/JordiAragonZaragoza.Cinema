namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class ReservationReadRepository<T, TId> : BaseReadRepository<T, TId>
        where T : class, IEntity<TId>
    {
        public ReservationReadRepository(ReservationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
