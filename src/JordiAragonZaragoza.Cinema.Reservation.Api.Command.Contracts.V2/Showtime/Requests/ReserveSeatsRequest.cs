namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed record class ReserveSeatsRequest(Guid ReservationId, Guid ShowtimeId, IEnumerable<Guid> SeatsIds);
}