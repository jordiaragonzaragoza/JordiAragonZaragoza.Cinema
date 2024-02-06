namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel;
    using Microsoft.Extensions.Logging;

    public class ReservationReadModelCachedSpecificationRepository<TReadModel> : BaseCachedSpecificationRepository<TReadModel>
        where TReadModel : class, IReadModel
    {
        public ReservationReadModelCachedSpecificationRepository(
            ReservationWriteContext dbContext,
            ILogger<ReservationReadModelCachedSpecificationRepository<TReadModel>> logger,
            ICacheService cacheService)
            : base(dbContext, logger, cacheService)
        {
        }
    }
}
