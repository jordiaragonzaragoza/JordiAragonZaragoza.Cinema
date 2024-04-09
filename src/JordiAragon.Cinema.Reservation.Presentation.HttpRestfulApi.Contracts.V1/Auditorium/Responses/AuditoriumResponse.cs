namespace JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Responses
{
    using System;

    public sealed record class AuditoriumResponse(Guid Id, string Name);
}