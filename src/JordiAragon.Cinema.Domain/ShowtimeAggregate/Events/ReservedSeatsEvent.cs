namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ReservedSeatsEvent(Ticket Ticket) : BaseDomainEvent;
}