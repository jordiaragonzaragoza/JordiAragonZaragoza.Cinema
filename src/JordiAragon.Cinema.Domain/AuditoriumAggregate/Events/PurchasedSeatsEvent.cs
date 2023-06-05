namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class PurchasedSeatsEvent(Ticket Ticket) : BaseDomainEvent;
}