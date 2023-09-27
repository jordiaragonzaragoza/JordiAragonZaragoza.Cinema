namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Auditorium.Responses
{
    using System;

    public record class SeatResponse(Guid Id, int Row, int SeatNumber);
}