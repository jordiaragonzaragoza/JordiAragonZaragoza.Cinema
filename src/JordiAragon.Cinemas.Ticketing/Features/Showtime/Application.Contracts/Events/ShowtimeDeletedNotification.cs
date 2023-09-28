namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Events
{
    using JordiAragon.SharedKernel.Application.Contracts.Events;

    public record class ShowtimeDeletedNotification(ShowtimeDeletedEvent Event)
        : BaseApplicationEventNotification<ShowtimeDeletedEvent>(Event);
}