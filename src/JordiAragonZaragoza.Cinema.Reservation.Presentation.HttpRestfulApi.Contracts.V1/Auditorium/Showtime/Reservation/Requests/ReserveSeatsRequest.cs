namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Reservation.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed record class ReserveSeatsRequest(Guid AuditoriumId, Guid ShowtimeId, IEnumerable<Guid> SeatsIds);
}