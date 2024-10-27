namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Requests
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class UserTicketsRequest(
        Guid UserId,
        Guid? ShowtimeId,
        DateTimeOffset? StartIntervalTimeOnUtc,
        DateTimeOffset? EndIntervalTimeOnUtc,
        string? AuditoriumName,
        string? MovieTitle,
        bool? IsPurchased) : PaginatedRequest;
}