namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Notifications
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class AuditoriumRemovedNotification(AuditoriumRemovedEvent Event)
        : BaseDomainEventNotification<AuditoriumRemovedEvent>(Event);
}