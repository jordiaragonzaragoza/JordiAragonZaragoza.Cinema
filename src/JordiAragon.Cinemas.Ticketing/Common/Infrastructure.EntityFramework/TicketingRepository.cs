namespace JordiAragon.Cinemas.Ticketing.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class TicketingRepository<T> : BaseRepository<T>
        where T : class, IAggregateRoot
    {
        public TicketingRepository(TicketingContext dbContext)
            : base(dbContext)
        {
        }
    }
}
