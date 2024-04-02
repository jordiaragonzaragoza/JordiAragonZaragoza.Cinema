#nullable enable
namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Requests
{
    using System;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;

    public sealed record class UserTicketsRequest(
        Guid UserId,
        Guid? ShowtimeId,
        DateTimeOffset? StartIntervalTimeOnUtc,
        DateTimeOffset? EndIntervalTimeOnUtc,
        string? AuditoriumName,
        string? MovieTitle,
        bool? IsPurchased) : PaginatedRequest;
}