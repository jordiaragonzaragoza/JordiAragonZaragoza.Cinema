namespace JordiAragon.Cinema.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Domain.Events.Services;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class CinemaDomainEventsDispatcher : BaseDomainEventsDispatcher
    {
        public CinemaDomainEventsDispatcher(
            CinemaContext context,
            IDomainEventsDispatcherService domainEventDispatcherService)
             : base(context, domainEventDispatcherService)
        {
        }
    }
}