namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class TicketingReadRepository<T, TId> : BaseReadRepository<T, TId>
        where T : class, IEntity<TId>
    {
        public TicketingReadRepository(TicketingContext dbContext)
            : base(dbContext)
        {
        }
    }
}
