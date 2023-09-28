namespace JordiAragon.Cinemas.Ticketing.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class TicketingReadRepository<T> : BaseReadRepository<T>
        where T : class
    {
        public TicketingReadRepository(TicketingContext dbContext)
            : base(dbContext)
        {
        }
    }
}
