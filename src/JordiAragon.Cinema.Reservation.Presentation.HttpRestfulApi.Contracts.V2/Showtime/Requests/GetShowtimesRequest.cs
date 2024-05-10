#nullable enable
namespace JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests
{
    using System;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class GetShowtimesRequest(
        Guid? AuditoriumId,
        Guid? MovieId,
        DateTime? StartTimeOnUtc,
        DateTime? EndTimeOnUtc,
        string? MovieTitle,
        string? AuditoriumName)
        : PaginatedRequest;
}