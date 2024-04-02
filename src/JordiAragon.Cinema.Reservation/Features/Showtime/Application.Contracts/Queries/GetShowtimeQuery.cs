namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetShowtimeQuery(
        Guid ShowtimeId)
        : IQuery<ShowtimeReadModel>, ICacheRequest
    {
        public string CacheKey
            => $"{ShowtimeConstants.CachePrefix}_{this.ShowtimeId}";

        public TimeSpan? AbsoluteExpirationInSeconds { get; }
    }
}