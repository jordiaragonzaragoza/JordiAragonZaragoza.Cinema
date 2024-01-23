namespace JordiAragon.Cinema.Reservation.Common.Infrastructure
{
    using JordiAragon.SharedKernel.Domain.Events.Services;
    using JordiAragon.SharedKernel.Infrastructure;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public class ReservationDomainEventsDispatcher : BaseDomainEventsDispatcher
    {
        public ReservationDomainEventsDispatcher(
            IWriteStore writeStore,
            IEventStore eventStore,
            IEventsDispatcherService domainEventDispatcherService)
             : base(writeStore, eventStore, domainEventDispatcherService)
        {
        }
    }
}