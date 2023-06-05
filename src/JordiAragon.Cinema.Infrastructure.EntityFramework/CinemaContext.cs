namespace JordiAragon.Cinema.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class CinemaContext : BaseContext
    {
        public CinemaContext(
            DbContextOptions<CinemaContext> options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
            : base(options, loggerFactory, hostEnvironment, auditableEntitySaveChangesInterceptor)
        {
        }

        public DbSet<Auditorium> Auditoriums => this.Set<Auditorium>();

        public DbSet<Showtime> Showtimes => this.Set<Showtime>();

        public DbSet<Movie> Movies => this.Set<Movie>();

        public DbSet<Ticket> Tickets => this.Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(InfrastructureEntityFrameworkAssemblyReference.Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}