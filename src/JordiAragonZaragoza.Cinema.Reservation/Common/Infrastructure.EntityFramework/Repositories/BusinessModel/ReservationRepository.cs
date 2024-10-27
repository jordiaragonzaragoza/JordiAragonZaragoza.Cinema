namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel
{
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Repositories.BusinessModel;

    public sealed class ReservationRepository<TAggregate, TId> : BaseRepository<TAggregate, TId>
        where TAggregate : BaseAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public ReservationRepository(ReservationBusinessModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}