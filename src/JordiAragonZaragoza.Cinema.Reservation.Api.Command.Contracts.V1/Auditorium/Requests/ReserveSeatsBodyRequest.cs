namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed record class ReserveSeatsBodyRequest(IEnumerable<Guid> SeatsIds);
}