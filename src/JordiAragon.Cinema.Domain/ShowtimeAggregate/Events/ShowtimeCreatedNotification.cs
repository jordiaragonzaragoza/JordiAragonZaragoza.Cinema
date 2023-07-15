namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeCreatedNotification : BaseDomainEventNotification<ShowtimeCreatedEvent>
    {
        public ShowtimeCreatedNotification(ShowtimeCreatedEvent @event)
            : base(@event)
        {
        }
    }
}