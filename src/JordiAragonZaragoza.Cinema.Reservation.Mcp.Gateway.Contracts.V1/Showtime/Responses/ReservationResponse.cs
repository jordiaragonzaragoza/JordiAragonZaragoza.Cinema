namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public sealed record class ReservationResponse(
        [Description("The Id of the reservation.")]
        Guid Id,
        [Description("The Id of the user who made the reservation.")]
        Guid UserId,
        [Description("The Id of the showtime for which the reservation was made.")]
        Guid ShowtimeId,
        [Description("The date and time of the showtime in UTC.")]
        DateTimeOffset SessionDateOnUtc,
        [Description("The name of the auditorium.")]
        string AuditoriumName,
        [Description("The title of the movie.")]
        string MovieTitle,
        [Description("The list of seats in the reservation.")]
        IEnumerable<SeatResponse> Seats,
        [Description("Indicates whether the reservation has been purchased.")]
        bool IsPurchased);
}