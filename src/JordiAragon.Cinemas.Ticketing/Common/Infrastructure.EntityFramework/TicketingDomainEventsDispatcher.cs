namespace JordiAragon.Cinemas.Ticketing.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Domain.Events.Services;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class TicketingDomainEventsDispatcher : BaseDomainEventsDispatcher
    {
        public TicketingDomainEventsDispatcher(
            TicketingContext context,
            IEventsDispatcherService domainEventDispatcherService)
             : base(context, domainEventDispatcherService)
        {
        }
    }
}