namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ReservedSeatsEvent(Ticket Ticket) : BaseDomainEvent;
}