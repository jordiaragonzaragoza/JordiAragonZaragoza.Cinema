namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium.Responses
{
    using System;

    public sealed record class SeatResponse(Guid Id, ushort Row, ushort SeatNumber);
}