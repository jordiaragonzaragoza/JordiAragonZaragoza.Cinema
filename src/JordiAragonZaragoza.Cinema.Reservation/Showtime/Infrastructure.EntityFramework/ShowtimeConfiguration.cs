namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// This configuration is obsolete when using showtime as event sourced aggregate.
    /// It is conserved as future reference.
    /// </summary>
    public sealed class ShowtimeConfiguration : BaseAggregateRootTypeConfiguration<Showtime, ShowtimeId>
    {
        public override void Configure(EntityTypeBuilder<Showtime> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            this.ConfigureShowtimesTable(builder);
            ConfigureShowtimesReservationsTable(builder);
        }

        private static void ConfigureShowtimesReservationsTable(EntityTypeBuilder<Showtime> builder)
        {
            builder.OwnsMany(showtime => showtime.Reservations, tb =>
            {
                tb.ToTable("ShowtimesReservations");

                tb.WithOwner().HasForeignKey(nameof(ShowtimeId));

                tb.HasKey(nameof(Reservation.Id), nameof(ShowtimeId));

                tb.Property(reservation => reservation.Id)
                  .HasColumnName(nameof(Reservation.Id))
                  .ValueGeneratedNever()
                  .HasConversion(reservationId => reservationId.Value, guidValue => new ReservationId(guidValue));

                tb.Property(reservation => reservation.UserId)
                .HasConversion(userId => userId.Value, value => new UserId(value))
                .HasColumnName(nameof(UserId));

                tb.Property(reservation => reservation.ReservationDateOnUtc)
                    .HasConversion(reservationDate => reservationDate.Value, value => new ReservationDate(value))
                    .HasColumnName(nameof(ReservationDate));

                tb.OwnsMany(reservation => reservation.Seats, reservationSeatBuilder =>
                {
                    reservationSeatBuilder.ToTable("ShowtimeReservationSeatIds");

                    reservationSeatBuilder.WithOwner().HasForeignKey(nameof(ReservationId), nameof(ShowtimeId));

                    reservationSeatBuilder.Property(seatId => seatId.Value)
                        .HasColumnName(nameof(SeatId))
                        .ValueGeneratedNever();
                });

                tb.Navigation(x => x.Seats).Metadata.SetField("seats");
                tb.Navigation(x => x.Seats).UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            builder.Metadata.FindNavigation(nameof(Showtime.Reservations))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureShowtimesTable(EntityTypeBuilder<Showtime> builder)
        {
            builder.ToTable("Showtimes");

            base.Configure(builder);

            builder.Property(showtime => showtime.Id)
                .ValueGeneratedNever()
                .HasConversion(showtimeId => showtimeId.Value, intValue => new ShowtimeId(intValue));

            builder.Property(showtime => showtime.AuditoriumId)
                .HasConversion(id => id.Value, value => new AuditoriumId(value));

            builder.Property(showtime => showtime.MovieId)
                .HasConversion(id => id.Value, value => new MovieId(value));

            builder.Property(showtime => showtime.SessionDateOnUtc)
                .HasConversion(
                    sessionDate => sessionDate.Value,
                    value => new SessionDate(value));
        }
    }
}