﻿namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class CreateShowtimeCommand(Guid AuditoriumId, Guid MovieId, DateTime SessionDateOnUtc)
        : ICommand<Guid>, IInvalidateCacheRequest
    {
        public string PrefixCacheKey => ShowtimeConstants.CachePrefix;
    }
}