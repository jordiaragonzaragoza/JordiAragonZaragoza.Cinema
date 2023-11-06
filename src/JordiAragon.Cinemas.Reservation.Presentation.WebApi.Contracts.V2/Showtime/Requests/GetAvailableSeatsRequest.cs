namespace JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests
{
    using System;

    public record class GetAvailableSeatsRequest(Guid ShowtimeId);
}