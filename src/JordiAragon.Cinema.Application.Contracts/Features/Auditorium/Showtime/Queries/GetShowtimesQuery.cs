namespace JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Showtime.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetShowtimesQuery(Guid AuditoriumId) : IQuery<IEnumerable<ShowtimeOutputDto>>, ICacheRequest
    {
        public string CacheKey => ShowtimeConstants.CachePrefix;

        public TimeSpan? AbsoluteExpirationInSeconds { get; }
    }
}