namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class GetShowtimesRequest(
        Guid? AuditoriumId,
        Guid? MovieId,
        DateTimeOffset? StartTimeOnUtc,
        DateTimeOffset? EndTimeOnUtc,
        string? MovieTitle,
        string? AuditoriumName)
        : PaginatedRequest;
}