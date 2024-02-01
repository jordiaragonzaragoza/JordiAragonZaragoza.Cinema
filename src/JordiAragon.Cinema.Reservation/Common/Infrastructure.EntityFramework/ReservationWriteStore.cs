namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    // NOTE: This class is required due to ReservationContext registration on DI.
    public class ReservationWriteStore : BaseWriteStore
    {
        public ReservationWriteStore(ReservationWriteContext context)
            : base(context)
        {
        }
    }
}