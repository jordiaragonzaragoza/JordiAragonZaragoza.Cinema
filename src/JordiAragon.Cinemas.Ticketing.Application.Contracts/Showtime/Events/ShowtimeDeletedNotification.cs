namespace JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Events
{
    using JordiAragon.SharedKernel.Application.Contracts.Events;

    public record class ShowtimeDeletedNotification(ShowtimeDeletedEvent Event)
        : BaseApplicationEventNotification<ShowtimeDeletedEvent>(Event);
}