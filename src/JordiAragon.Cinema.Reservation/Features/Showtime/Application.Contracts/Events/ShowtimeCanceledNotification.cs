namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events
{
    using JordiAragon.SharedKernel.Application.Contracts.Events;

    public sealed record class ShowtimeCanceledNotification(ShowtimeCanceledEvent Event)
        : BaseApplicationEventNotification<ShowtimeCanceledEvent>(Event);
}