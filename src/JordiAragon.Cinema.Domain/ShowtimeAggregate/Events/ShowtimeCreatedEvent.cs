namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeCreatedEvent(Showtime Showtime, AuditoriumId AuditoriumId) : BaseDomainEvent;
}