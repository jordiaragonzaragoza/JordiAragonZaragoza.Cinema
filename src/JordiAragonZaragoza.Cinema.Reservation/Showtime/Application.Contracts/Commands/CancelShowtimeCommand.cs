namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class CancelShowtimeCommand(Guid ShowtimeId) : ICommand ////, IInvalidateCacheRequest // TODO: Enable cache invalidation when implementing cache
    {
        ////public string PrefixCacheKey => ShowtimeConstants.CachePrefix;
    }
}