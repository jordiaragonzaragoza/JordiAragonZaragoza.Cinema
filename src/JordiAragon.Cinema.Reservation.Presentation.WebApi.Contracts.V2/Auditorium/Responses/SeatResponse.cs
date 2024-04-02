namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses
{
    using System;

    public sealed record class SeatResponse(Guid Id, int Row, int SeatNumber);
}