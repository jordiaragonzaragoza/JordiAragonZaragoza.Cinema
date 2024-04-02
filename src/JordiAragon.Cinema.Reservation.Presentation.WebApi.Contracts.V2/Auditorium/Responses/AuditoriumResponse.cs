namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses
{
    using System;

    public sealed record class AuditoriumResponse(Guid Id, string Name);
}