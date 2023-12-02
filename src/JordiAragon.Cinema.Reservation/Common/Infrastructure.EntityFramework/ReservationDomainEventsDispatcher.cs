namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Domain.Events.Services;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class ReservationDomainEventsDispatcher : BaseDomainEventsDispatcher
    {
        public ReservationDomainEventsDispatcher(
            ReservationContext context,
            IEventsDispatcherService domainEventDispatcherService)
             : base(context, domainEventDispatcherService)
        {
        }
    }
}