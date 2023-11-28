namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories;
    using Microsoft.Extensions.Logging;

    public class ReservationCachedSpecificationRepository<TAggregate, TId> : BaseCachedSpecificationRepository<TAggregate, TId>
        where TAggregate : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public ReservationCachedSpecificationRepository(
            ReservationContext dbContext,
            ILogger<ReservationCachedSpecificationRepository<TAggregate, TId>> logger,
            ICacheService cacheService)
            : base(dbContext, logger, cacheService)
        {
        }
    }
}
