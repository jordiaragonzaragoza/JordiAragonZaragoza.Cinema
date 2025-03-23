namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Notifications
{
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class MovieRemovedNotification(MovieRemovedEvent Event)
        : BaseDomainEventNotification<MovieRemovedEvent>(Event);
}