namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class ReservationRepository<T, TId> : BaseRepository<T, TId>
        where T : class, IAggregateRoot<TId>
    {
        public ReservationRepository(ReservationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
