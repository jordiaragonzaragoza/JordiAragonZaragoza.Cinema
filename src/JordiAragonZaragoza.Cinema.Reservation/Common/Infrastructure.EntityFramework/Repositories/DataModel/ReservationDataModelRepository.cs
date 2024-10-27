namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel
{
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Repositories.DataModel;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Interfaces;

    public sealed class ReservationDataModelRepository<TDataEntity> : BaseRepository<TDataEntity>
        where TDataEntity : class, IDataEntity
    {
        public ReservationDataModelRepository(ReservationBusinessModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}