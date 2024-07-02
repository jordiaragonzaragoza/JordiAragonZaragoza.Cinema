namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications
{
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeCanceledNotification(ShowtimeCanceledEvent Event)
        : BaseDomainEventNotification<ShowtimeCanceledEvent>(Event);
}