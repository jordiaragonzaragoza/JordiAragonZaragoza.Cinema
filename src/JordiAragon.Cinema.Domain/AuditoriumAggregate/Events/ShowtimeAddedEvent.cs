namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeAddedEvent(Showtime Showtime, Auditorium Auditorium) : BaseDomainEvent;
}