namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel;

    public class ReservationReadModelRepository<TReadModel> : BaseRepository<TReadModel>
        where TReadModel : class, IReadModel
    {
        public ReservationReadModelRepository(ReservationReadContext dbContext)
            : base(dbContext)
        {
        }
    }
}
