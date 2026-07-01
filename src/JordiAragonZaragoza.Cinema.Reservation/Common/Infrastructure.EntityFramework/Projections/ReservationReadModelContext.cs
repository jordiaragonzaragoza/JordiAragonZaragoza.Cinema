namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Context;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class ReservationReadModelContext : BaseReadModelContext
    {
        public ReservationReadModelContext(
            DbContextOptions<ReservationReadModelContext> options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment,
            TenantReadModelSaveChangesInterceptor tenantInterceptor)
            : base(options, loggerFactory, hostEnvironment, tenantInterceptor)
        {
        }

        public DbSet<AuditoriumReadModel> Auditoriums => this.Set<AuditoriumReadModel>();

        public DbSet<MovieReadModel> Movies => this.Set<MovieReadModel>();

        public DbSet<UserReadModel> Users => this.Set<UserReadModel>();

        public DbSet<UserAuthorizationReadModel> UsersAuthorizations => this.Set<UserAuthorizationReadModel>();

        public DbSet<ShowtimeReadModel> Showtimes => this.Set<ShowtimeReadModel>();

        public DbSet<AvailableSeatReadModel> AvailableSeats => this.Set<AvailableSeatReadModel>();

        public DbSet<ReservationReadModel> Reservations => this.Set<ReservationReadModel>();

        public DbSet<TenantReadModel> Tenants => this.Set<TenantReadModel>();

        public DbSet<PartitionReadModel> Partitions => this.Set<PartitionReadModel>();

        public DbSet<CinemaReadModel> Cinemas => this.Set<CinemaReadModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new MovieReadModelConfiguration(this));
            modelBuilder.ApplyConfiguration(new AuditoriumReadModelConfiguration(this));
            modelBuilder.ApplyConfiguration(new ShowtimeReadModelConfiguration(this));
            modelBuilder.ApplyConfiguration(new AvailableSeatReadModelConfiguration(this));
            modelBuilder.ApplyConfiguration(new ReservationReadModelConfiguration(this));
            modelBuilder.ApplyConfiguration(new UserReadModelConfiguration(this));
            modelBuilder.ApplyConfiguration(new UserAuthorizationReadModelConfiguration(this));
            modelBuilder.ApplyConfiguration(new TenantReadModelConfiguration(this));
            modelBuilder.ApplyConfiguration(new PartitionReadModelConfiguration(this));
            modelBuilder.ApplyConfiguration(new CinemaReadModelConfiguration(this));

            base.OnModelCreating(modelBuilder);
        }
    }
}