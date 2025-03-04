namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Notifications
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class AuditoriumCreatedNotification(AuditoriumCreatedEvent Event)
        : BaseDomainEventNotification<AuditoriumCreatedEvent>(Event);
}