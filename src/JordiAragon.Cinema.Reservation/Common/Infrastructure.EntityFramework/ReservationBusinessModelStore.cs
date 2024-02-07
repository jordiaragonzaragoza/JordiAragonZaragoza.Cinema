namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    // NOTE: This class is required due to ReservationContext registration on DI.
    public class ReservationBusinessModelStore : BaseBusinessModelStore
    {
        public ReservationBusinessModelStore(ReservationBusinessModelContext context)
            : base(context)
        {
        }
    }
}