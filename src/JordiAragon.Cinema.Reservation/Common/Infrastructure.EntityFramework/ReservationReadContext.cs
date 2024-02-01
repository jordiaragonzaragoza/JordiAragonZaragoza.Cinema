namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class ReservationReadContext : BaseReadContext
    {
        public ReservationReadContext(
            DbContextOptions<ReservationReadContext> options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment)
            : base(options, loggerFactory, hostEnvironment)
        {
        }

        ////public DbSet<Showtime> Showtimes => this.Set<Showtime>();

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ShowtimeConfiguration());

            base.OnModelCreating(modelBuilder);
        }*/
    }
}