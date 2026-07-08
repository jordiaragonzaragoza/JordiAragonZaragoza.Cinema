namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections
{
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel;
    using Microsoft.Extensions.Logging;

    public sealed class ReservationReadModelCachedSpecificationRepository<TReadModel> : BaseCachedSpecificationRepository<TReadModel>
        where TReadModel : class, IReadModel
    {
        public ReservationReadModelCachedSpecificationRepository(
            ReservationReadModelContext dbContext,
            ILogger<ReservationReadModelCachedSpecificationRepository<TReadModel>> logger,
            ICacheService cacheService)
            : base(dbContext, logger, cacheService)
        {
        }
    }
}