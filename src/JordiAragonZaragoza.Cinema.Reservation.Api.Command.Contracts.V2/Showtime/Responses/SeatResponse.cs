namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses
{
    using System;

    public sealed record class SeatResponse(Guid Id, ushort Row, ushort SeatNumber);
}