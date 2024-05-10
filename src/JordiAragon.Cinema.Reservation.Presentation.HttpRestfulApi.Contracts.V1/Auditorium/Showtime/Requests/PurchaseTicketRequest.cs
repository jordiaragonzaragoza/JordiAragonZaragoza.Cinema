namespace JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Requests
{
    using System;

    public sealed record class PurchaseTicketRequest(Guid AuditoriumId, Guid ShowtimeId, Guid TicketId, bool IsPurchased);
}