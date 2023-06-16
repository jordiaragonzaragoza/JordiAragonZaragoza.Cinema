namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using System;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeCreatedEvent(
        ShowtimeId ShowtimeId,
        MovieId MovieId,
        DateTime SessionDateOnUtc,
        AuditoriumId AuditoriumId)
        : BaseDomainEvent;
}