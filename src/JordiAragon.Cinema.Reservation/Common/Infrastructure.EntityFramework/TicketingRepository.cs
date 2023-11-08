namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class TicketingRepository<T, TId> : BaseRepository<T, TId>
        where T : class, IAggregateRoot<TId>
    {
        public TicketingRepository(TicketingContext dbContext)
            : base(dbContext)
        {
        }
    }
}
