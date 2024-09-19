namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.DataModel
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.DataModel;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;
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