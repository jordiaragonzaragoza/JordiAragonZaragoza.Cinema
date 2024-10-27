namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed record class ReserveSeatsRequest(Guid ShowtimeId, IEnumerable<Guid> SeatsIds);
}