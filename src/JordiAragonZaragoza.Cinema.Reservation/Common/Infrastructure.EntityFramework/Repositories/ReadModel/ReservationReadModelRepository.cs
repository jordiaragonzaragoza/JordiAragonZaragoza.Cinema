namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel
{
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel;

    public sealed class ReservationReadModelRepository<TReadModel> : BaseRepository<TReadModel>
        where TReadModel : class, IReadModel
    {
        public ReservationReadModelRepository(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}