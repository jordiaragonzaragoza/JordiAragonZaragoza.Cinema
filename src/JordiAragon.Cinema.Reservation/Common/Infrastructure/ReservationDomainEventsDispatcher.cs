namespace JordiAragon.Cinema.Reservation.Common.Infrastructure
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Events.Services;
    using JordiAragon.SharedKernel.Infrastructure;

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