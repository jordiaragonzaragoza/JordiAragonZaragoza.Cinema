namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed record class ReserveSeatsRequest(Guid AuditoriumId, Guid ShowtimeId, IEnumerable<Guid> SeatsIds);
}