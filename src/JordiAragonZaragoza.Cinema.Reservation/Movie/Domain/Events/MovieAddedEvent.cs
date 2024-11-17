namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class MovieAddedEvent(
        Guid AggregateId,
        string Title,
        TimeSpan Runtime,
        DateTimeOffset StartingExhibitionPeriodOnUtc,
        DateTimeOffset EndOfExhibitionPeriodOnUtc)
        : BaseDomainEvent(AggregateId);
}