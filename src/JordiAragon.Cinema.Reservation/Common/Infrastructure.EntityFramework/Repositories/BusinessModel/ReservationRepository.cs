namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.BusinessModel;

    public class ReservationRepository<TAggregate, TId> : BaseRepository<TAggregate, TId>
        where TAggregate : BaseAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public ReservationRepository(ReservationWriteContext dbContext)
            : base(dbContext)
        {
        }
    }
}
