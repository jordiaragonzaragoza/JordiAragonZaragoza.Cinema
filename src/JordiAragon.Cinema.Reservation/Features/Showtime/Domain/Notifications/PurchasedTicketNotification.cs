namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications
{
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class PurchasedTicketNotification(PurchasedTicketEvent Event)
        : BaseDomainEventNotification<PurchasedTicketEvent>(Event);
}