namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;
    using Microsoft.Extensions.Logging;

    public class TicketingCachedRepository<T, TId> : BaseCachedRepository<T, TId>
        where T : class, IAggregateRoot<TId>
    {
        public TicketingCachedRepository(
            TicketingContext dbContext,
            ILogger<TicketingCachedRepository<T, TId>> logger,
            ICacheService cacheService)
            : base(dbContext, logger, cacheService)
        {
        }
    }
}
