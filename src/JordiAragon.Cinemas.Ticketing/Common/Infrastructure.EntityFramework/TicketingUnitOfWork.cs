namespace JordiAragon.Cinemas.Ticketing.Common.Infrastructure.EntityFramework
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