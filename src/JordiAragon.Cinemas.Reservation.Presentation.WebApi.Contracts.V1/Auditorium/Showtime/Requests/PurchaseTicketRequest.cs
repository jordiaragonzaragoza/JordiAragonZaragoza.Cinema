namespace JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests
{
    using System;

    public record class PurchaseTicketRequest(Guid AuditoriumId, Guid ShowtimeId, Guid TicketId, bool IsPurchased);
}