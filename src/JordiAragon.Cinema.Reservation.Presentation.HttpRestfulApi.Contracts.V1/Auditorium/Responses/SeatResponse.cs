namespace JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Responses
{
    using System;

    public sealed record class SeatResponse(Guid Id, int Row, int SeatNumber);
}