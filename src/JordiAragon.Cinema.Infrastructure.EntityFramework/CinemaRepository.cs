namespace JordiAragon.Cinema.Infrastructure.EntityFramework
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;

    public class CinemaRepository<T> : BaseRepository<T>
        where T : class, IAggregateRoot
    {
        public CinemaRepository(CinemaContext dbContext)
            : base(dbContext)
        {
        }
    }
}
