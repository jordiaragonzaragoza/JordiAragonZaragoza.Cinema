namespace JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeCreatedNotification(ShowtimeCreatedEvent Event)
        : BaseDomainEventNotification<ShowtimeCreatedEvent>(Event);
}