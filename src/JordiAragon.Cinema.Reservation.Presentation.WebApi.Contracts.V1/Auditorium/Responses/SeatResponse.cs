namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Responses
{
    using System;

    public record class SeatResponse(Guid Id, int Row, int SeatNumber);
}