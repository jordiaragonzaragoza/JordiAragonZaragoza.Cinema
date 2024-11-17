namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Notifications
{
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class ShowtimeScheduledNotification(ShowtimeScheduledEvent Event)
        : BaseDomainEventNotification<ShowtimeScheduledEvent>(Event);
}