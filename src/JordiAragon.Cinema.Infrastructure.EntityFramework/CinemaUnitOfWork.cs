namespace JordiAragon.Cinema.Infrastructure.EntityFramework
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