namespace JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class TicketingUnitOfWork : BaseUnitOfWork
    {
        public TicketingUnitOfWork(
            TicketingContext context)
            : base(context)
        {
        }
    }
}