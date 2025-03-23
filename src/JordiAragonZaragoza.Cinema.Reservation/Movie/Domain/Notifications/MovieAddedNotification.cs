namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Notifications
{
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class MovieAddedNotification(MovieAddedEvent Event)
        : BaseDomainEventNotification<MovieAddedEvent>(Event);
}