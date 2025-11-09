namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class UserReservationsRequest(
        Guid UserId,
        Guid? ShowtimeId,
        DateTimeOffset? StartIntervalTimeOnUtc,
        DateTimeOffset? EndIntervalTimeOnUtc,
        string? AuditoriumName,
        string? MovieTitle,
        bool? IsPurchased) : PaginatedRequest;
}