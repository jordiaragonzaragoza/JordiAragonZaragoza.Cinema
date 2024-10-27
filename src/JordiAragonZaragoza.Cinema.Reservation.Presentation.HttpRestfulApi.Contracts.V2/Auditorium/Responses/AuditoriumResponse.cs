namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses
{
    using System;

    public sealed record class AuditoriumResponse(Guid Id, string Name);
}