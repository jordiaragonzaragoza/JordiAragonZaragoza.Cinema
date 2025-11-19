namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests
{
    using System;

    public sealed record class UserReservationsRequest(
        Guid UserId,
        Guid? ShowtimeId,
        DateTimeOffset? StartIntervalTimeOnUtc,
        DateTimeOffset? EndIntervalTimeOnUtc,
        string? AuditoriumName,
        string? MovieTitle,
        bool? IsPurchased);
}