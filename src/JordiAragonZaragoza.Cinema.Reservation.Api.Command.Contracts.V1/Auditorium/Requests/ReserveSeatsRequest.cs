namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed record class ReserveSeatsRequest(Guid AuditoriumId, Guid ShowtimeId, IEnumerable<Guid> SeatsIds);
}