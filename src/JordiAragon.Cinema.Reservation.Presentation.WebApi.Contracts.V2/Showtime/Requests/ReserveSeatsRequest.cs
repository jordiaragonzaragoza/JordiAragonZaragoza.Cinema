namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests
{
    using System;
    using System.Collections.Generic;

    public record class ReserveSeatsRequest(Guid ShowtimeId, IEnumerable<Guid> SeatsIds);
}