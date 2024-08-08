namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetShowtimesQuery(
        Guid? AuditoriumId,
        string? AuditoriumName,
        Guid? MovieId,
        string? MovieTitle,
        DateTimeOffset? StartTimeOnUtc,
        DateTimeOffset? EndTimeOnUtc,
        int PageNumber,
        int PageSize)
        : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<ShowtimeReadModel>>, ICacheRequest
    {
        public string CacheKey
            => $"{ShowtimeConstants.CachePrefix}_{this.AuditoriumId}_{this.AuditoriumName}_{this.MovieId}_{this.MovieTitle}_{this.StartTimeOnUtc}_{this.EndTimeOnUtc}_{this.PageNumber}_{this.PageSize}";

        public TimeSpan? AbsoluteExpirationInSeconds { get; }
    }
}