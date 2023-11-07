namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests
{
    using System;

    public record class GetShowtimesRequest(Guid AuditoriumId, Guid? MovieId, DateTime? StartTimeOnUtc, DateTime? EndTimeOnUtc);
}