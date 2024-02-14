#nullable enable
namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests
{
    using System;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;

    public record class GetShowtimesRequest(
        Guid? AuditoriumId,
        Guid? MovieId,
        DateTime? StartTimeOnUtc,
        DateTime? EndTimeOnUtc,
        string? MovieTitle,
        string? AuditoriumName)
        : PaginatedRequest;
}