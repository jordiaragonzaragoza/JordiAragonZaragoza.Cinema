namespace JordiAragon.Cinema.Reservation.Movie.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class MovieCreatedEvent(
        Guid MovieId,
        string Title,
        TimeSpan Runtime,
        DateTime StartingExhibitionPeriodOnUtc,
        DateTime EndOfExhibitionPeriodOnUtc)
        : BaseDomainEvent(MovieId);
}