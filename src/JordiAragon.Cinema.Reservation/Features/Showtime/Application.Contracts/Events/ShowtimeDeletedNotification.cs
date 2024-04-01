namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events
{
    using JordiAragon.SharedKernel.Application.Contracts.Events;

    public sealed record class ShowtimeDeletedNotification(ShowtimeDeletedEvent Event)
        : BaseApplicationEventNotification<ShowtimeDeletedEvent>(Event);
}