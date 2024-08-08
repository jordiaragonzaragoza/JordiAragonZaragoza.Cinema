namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries
{
    using System;

    public sealed record class SeatOutputDto(Guid Id, ushort Row, ushort SeatNumber);
}