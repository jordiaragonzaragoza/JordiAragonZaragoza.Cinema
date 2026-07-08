namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections
{
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Contracts;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Repositories.DataModel;

    public sealed class ReservationDataModelRepository<TDataEntity> : BaseRepository<TDataEntity>
        where TDataEntity : class, IDataEntity
    {
        public ReservationDataModelRepository(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}