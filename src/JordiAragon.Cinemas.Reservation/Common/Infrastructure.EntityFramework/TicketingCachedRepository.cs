namespace JordiAragon.Cinemas.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;
    using Microsoft.Extensions.Logging;

    public class TicketingCachedRepository<T> : BaseCachedRepository<T>
        where T : class, IAggregateRoot
    {
        public TicketingCachedRepository(
            TicketingContext dbContext,
            ILogger<TicketingCachedRepository<T>> logger,
            ICacheService cacheService)
            : base(dbContext, logger, cacheService)
        {
        }
    }
}
