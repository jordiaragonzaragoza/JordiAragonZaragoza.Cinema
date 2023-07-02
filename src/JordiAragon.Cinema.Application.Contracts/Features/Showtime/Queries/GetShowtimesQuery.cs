namespace JordiAragon.Cinema.Application.Contracts.Features.Showtime.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetShowtimesQuery(Guid AuditoriumId) : IQuery<IEnumerable<ShowtimeOutputDto>>, ICacheRequest // TODO: Complete with optional parameters: Guid? MovieId, DateTime? StartTimeOnUtc, DateTime? EndTimeOnUtc
    {
        public string CacheKey => ShowtimeConstants.CachePrefix;

        public TimeSpan? AbsoluteExpirationInSeconds { get; }
    }
}