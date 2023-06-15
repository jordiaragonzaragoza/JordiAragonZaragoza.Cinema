namespace JordiAragon.Cinema.Domain.MovieAggregate.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class MovieCreatedEvent(Movie Movie) : BaseDomainEvent;
}