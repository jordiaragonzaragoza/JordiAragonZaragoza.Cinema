namespace JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class CinemaUnitOfWork : BaseUnitOfWork
    {
        public CinemaUnitOfWork(
            CinemaContext context)
            : base(context)
        {
        }
    }
}