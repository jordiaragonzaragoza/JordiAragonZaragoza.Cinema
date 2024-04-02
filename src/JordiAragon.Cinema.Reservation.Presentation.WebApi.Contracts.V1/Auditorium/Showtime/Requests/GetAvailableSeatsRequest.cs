namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests
{
    using System;

    public sealed record class GetAvailableSeatsRequest(Guid AuditoriumId, Guid ShowtimeId);
}