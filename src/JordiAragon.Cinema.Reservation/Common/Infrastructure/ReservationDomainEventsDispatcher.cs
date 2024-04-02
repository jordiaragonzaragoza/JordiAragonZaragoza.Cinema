namespace JordiAragon.Cinema.Reservation.Common.Infrastructure
{
    using JordiAragon.SharedKernel.Domain.Events.Services;
    using JordiAragon.SharedKernel.Infrastructure;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public sealed class ReservationDomainEventsDispatcher : BaseDomainEventsDispatcher
    {
        public ReservationDomainEventsDispatcher(
            IBusinessModelStore businessModelStore,
            IEventStore eventStore,
            IEventsDispatcherService domainEventDispatcherService)
             : base(businessModelStore, eventStore, domainEventDispatcherService)
        {
        }
    }
}