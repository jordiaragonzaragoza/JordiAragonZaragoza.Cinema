namespace JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;
    using Microsoft.Extensions.Logging;

    public class CinemaCachedRepository<T> : BaseCachedRepository<T>
        where T : class, IAggregateRoot
    {
        public CinemaCachedRepository(
            CinemaContext dbContext,
            ILogger<CinemaCachedRepository<T>> logger,
            ICacheService cacheService)
            : base(dbContext, logger, cacheService)
        {
        }
    }
}
