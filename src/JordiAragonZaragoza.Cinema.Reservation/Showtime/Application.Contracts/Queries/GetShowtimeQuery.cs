namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetShowtimeQuery(
        Guid ShowtimeId)
        : IQuery<ShowtimeReadModel>, ICacheRequest
    {
        public string CacheKey
            => $"{ShowtimeConstants.CachePrefix}_{this.ShowtimeId}";

        public TimeSpan? AbsoluteExpirationInSeconds { get; }
    }
}