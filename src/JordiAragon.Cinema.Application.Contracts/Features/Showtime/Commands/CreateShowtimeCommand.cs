namespace JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class CreateShowtimeCommand : ICommand<Guid>, IInvalidateCacheRequest
    {
        public string PrefixCacheKey => ShowtimeConstants.CachePrefix;

        public Guid AuditoriumId { get; set; } // This set is required to be wired post mapping.

        public Guid MovieId { get; init; }

        public DateTime SessionDateOnUtc { get; init; }
    }
}