namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework;
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

        public DbSet<AuditoriumReadModel> Auditoriums => this.Set<AuditoriumReadModel>();

        public DbSet<MovieReadModel> Movies => this.Set<MovieReadModel>();

        public DbSet<ShowtimeReadModel> Showtimes => this.Set<ShowtimeReadModel>();

        public DbSet<AvailableSeatReadModel> AvailableSeats => this.Set<AvailableSeatReadModel>();

        public DbSet<ReservationReadModel> Reservations => this.Set<ReservationReadModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new MovieReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new AuditoriumReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new ShowtimeReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new AvailableSeatReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationReadModelConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}