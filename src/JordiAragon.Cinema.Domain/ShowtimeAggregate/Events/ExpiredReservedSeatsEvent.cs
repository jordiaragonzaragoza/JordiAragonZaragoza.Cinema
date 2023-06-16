namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ExpiredReservedSeatsEvent(TicketId TicketId) : BaseDomainEvent;
}