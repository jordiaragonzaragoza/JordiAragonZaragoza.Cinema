namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections
{
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework;

    public sealed class ReservationProjectionsStore : BaseProjectionsStore
    {
        public ReservationProjectionsStore(ReservationReadModelContext context)
            : base(context)
        {
        }
    }
}