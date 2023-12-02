namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class ReservationUnitOfWork : BaseUnitOfWork
    {
        public ReservationUnitOfWork(
            ReservationContext context)
            : base(context)
        {
        }
    }
}