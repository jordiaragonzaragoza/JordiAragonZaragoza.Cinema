namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class ReservationWriteStore : BaseWriteStore
    {
        public ReservationWriteStore(ReservationContext context)
            : base(context)
        {
        }
    }
}