namespace JordiAragon.Cinemas.Ticketing.Showtime.Domain.Notifications
{
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeCreatedNotification(ShowtimeCreatedEvent Event)
        : BaseDomainEventNotification<ShowtimeCreatedEvent>(Event);
}