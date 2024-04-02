namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class DeleteShowtimeCommand(Guid ShowtimeId) : ICommand, IInvalidateCacheRequest
    {
        public string PrefixCacheKey => ShowtimeConstants.CachePrefix;
    }
}