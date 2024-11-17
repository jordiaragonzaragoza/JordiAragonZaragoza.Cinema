namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Context;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class ReservationReadModelContext : BaseReadModelContext
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

        public DbSet<TicketReadModel> Tickets => this.Set<TicketReadModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new ShowtimeReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new AvailableSeatReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new TicketReadModelConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}