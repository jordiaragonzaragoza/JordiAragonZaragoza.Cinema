namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class ScheduleShowtimeCommand(Guid ShowtimeId, Guid AuditoriumId, Guid MovieId, DateTimeOffset SessionDateOnUtc)
        : ICommand, IInvalidateCacheRequest
    {
        public string PrefixCacheKey => ShowtimeConstants.CachePrefix;
    }
}