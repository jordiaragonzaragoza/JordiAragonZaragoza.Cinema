namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Responses
{
    using System;

    public sealed record class SeatResponse(Guid Id, ushort Row, ushort SeatNumber);
}