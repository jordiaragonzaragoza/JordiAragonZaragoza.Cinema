namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel
{
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.DataModel;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public sealed class ReservationDataModelRepository<TDataEntity> : BaseRepository<TDataEntity>
        where TDataEntity : class, IDataEntity
    {
        public ReservationDataModelRepository(ReservationBusinessModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}
