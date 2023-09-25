namespace JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class CinemaReadRepository<T> : BaseReadRepository<T>
        where T : class
    {
        public CinemaReadRepository(CinemaContext dbContext)
            : base(dbContext)
        {
        }
    }
}
