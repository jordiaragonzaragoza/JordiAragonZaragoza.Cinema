namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications
{
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ExpiredReservedSeatsNotification(ExpiredReservedSeatsEvent Event)
        : BaseDomainEventNotification<ExpiredReservedSeatsEvent>(Event);
}