namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel
{
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel;

    public sealed class ReservationReadModelRepository<TReadModel> : BaseRepository<TReadModel>
        where TReadModel : class, IReadModel
    {
        public ReservationReadModelRepository(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}