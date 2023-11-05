namespace JordiAragon.Cinemas.Reservation.Showtime.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class PurchasedTicketEvent(Guid ShowtimeId, Guid TicketId) : BaseDomainEvent(ShowtimeId);
}