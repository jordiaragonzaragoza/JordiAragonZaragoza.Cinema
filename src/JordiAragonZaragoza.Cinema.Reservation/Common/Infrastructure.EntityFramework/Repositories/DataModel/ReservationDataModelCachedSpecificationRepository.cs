namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.DataModel
{
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Repositories.DataModel;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Interfaces;
    using Microsoft.Extensions.Logging;

    public sealed class ReservationDataModelCachedSpecificationRepository<TDataModel> : BaseCachedSpecificationRepository<TDataModel>
        where TDataModel : class, IDataEntity
    {
        public ReservationDataModelCachedSpecificationRepository(
            ReservationBusinessModelContext dbContext,
            ILogger<ReservationDataModelCachedSpecificationRepository<TDataModel>> logger,
            ICacheService cacheService)
            : base(dbContext, logger, cacheService)
        {
        }
    }
}