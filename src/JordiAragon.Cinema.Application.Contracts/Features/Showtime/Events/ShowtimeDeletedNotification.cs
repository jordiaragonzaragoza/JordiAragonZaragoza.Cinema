namespace JordiAragon.Cinema.Application.Contracts.Features.Showtime.Events
{
    using JordiAragon.SharedKernel.Application.Contracts.Events;

    public record class ShowtimeDeletedNotification(ShowtimeDeletedEvent Event)
        : BaseApplicationEventNotification<ShowtimeDeletedEvent>(Event);
}