namespace JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Ticket.Responses
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Responses;

    public sealed record class TicketResponse(
        Guid Id,
        DateTimeOffset SessionDateOnUtc,
        string AuditoriumName,
        string MovieTitle,
        IEnumerable<SeatResponse> Seats,
        bool IsPurchased);
}