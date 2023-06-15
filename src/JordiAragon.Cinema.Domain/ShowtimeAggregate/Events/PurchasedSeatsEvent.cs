namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class PurchasedSeatsEvent(Ticket Ticket) : BaseDomainEvent;
}