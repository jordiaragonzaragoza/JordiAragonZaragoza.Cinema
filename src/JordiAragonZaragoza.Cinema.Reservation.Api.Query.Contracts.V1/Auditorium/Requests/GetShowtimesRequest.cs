namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium.Requests
{
    using System;

    public sealed record class GetShowtimesRequest(Guid AuditoriumId);
}