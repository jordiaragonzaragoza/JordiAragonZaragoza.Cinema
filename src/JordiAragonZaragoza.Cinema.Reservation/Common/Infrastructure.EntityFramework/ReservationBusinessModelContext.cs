namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Context;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class ReservationBusinessModelContext : BaseBusinessModelContext
    {
        public ReservationBusinessModelContext(
            DbContextOptions<ReservationBusinessModelContext> options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment,
            SoftDeleteEntitySaveChangesInterceptor softDeleteEntitySaveChangesInterceptor)
            : base(options, loggerFactory, hostEnvironment, softDeleteEntitySaveChangesInterceptor)
        {
        }

        public DbSet<User> Users => this.Set<User>();

        public DbSet<Auditorium> Auditoriums => this.Set<Auditorium>();

        public DbSet<Movie> Movies => this.Set<Movie>();

        public DbSet<Showtime> Showtimes => this.Set<Showtime>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Guard.Against.Null(modelBuilder, nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AuditoriumConfiguration());
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new ShowtimeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}