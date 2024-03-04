namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class ReservationReadModelContext : BaseReadModelContext
    {
        public ReservationReadModelContext(
            DbContextOptions<ReservationReadModelContext> options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment)
            : base(options, loggerFactory, hostEnvironment)
        {
        }

        public DbSet<ShowtimeReadModel> Showtimes => this.Set<ShowtimeReadModel>();

        public DbSet<AvailableSeatReadModel> AvailableSeats => this.Set<AvailableSeatReadModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ShowtimeReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new AvailableSeatReadModelConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}