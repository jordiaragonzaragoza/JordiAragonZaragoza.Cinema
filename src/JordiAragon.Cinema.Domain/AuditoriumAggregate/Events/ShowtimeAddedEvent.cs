namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Events
{
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeAddedEvent(ShowtimeId ShowtimeId, Auditorium Auditorium) : BaseDomainEvent;
}