namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories;
    using Microsoft.Extensions.Logging;

    public class ReservationCachedSpecificationRepository<TAggregate, TId, TIdType> : BaseCachedSpecificationRepository<TAggregate, TId, TIdType>
        where TAggregate : class, IAggregateRoot<TId, TIdType>
        where TId : class, IEntityId<TIdType>
    {
        public ReservationCachedSpecificationRepository(
            ReservationContext dbContext,
            ILogger<ReservationCachedSpecificationRepository<TAggregate, TId, TIdType>> logger,
            ICacheService cacheService)
            : base(dbContext, logger, cacheService)
        {
        }
    }
}
