namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework;

    // NOTE: This class is required due to ReservationContext registration on DI.
    public sealed class ReservationBusinessModelStore : BaseBusinessModelStore
    {
        public ReservationBusinessModelStore(ReservationBusinessModelContext context)
            : base(context)
        {
        }
    }
}