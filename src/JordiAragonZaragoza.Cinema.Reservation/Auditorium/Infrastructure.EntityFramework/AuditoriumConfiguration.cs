namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class AuditoriumConfiguration : BaseAggregateRootTypeConfiguration<Auditorium, AuditoriumId>
    {
        public override void Configure(EntityTypeBuilder<Auditorium> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            this.ConfigureAuditoriumsTable(builder);

            ConfigureAuditoriumsShowtimeIdsTable(builder);

            ConfigureAuditoriumsSeatsTable(builder);
        }

        private static void ConfigureAuditoriumsShowtimeIdsTable(EntityTypeBuilder<Auditorium> builder)
        {
            builder.OwnsMany(auditorium => auditorium.ActiveShowtimes, sib =>
            {
                sib.ToTable("AuditoriumsActiveShowtimeIds");

                sib.WithOwner().HasForeignKey(nameof(AuditoriumId));

                sib.Property(showtimeId => showtimeId.Value)
                .ValueGeneratedNever()
                .HasColumnName(nameof(ShowtimeId));
            });

            builder.Metadata.FindNavigation(nameof(Auditorium.ActiveShowtimes))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private static void ConfigureAuditoriumsSeatsTable(EntityTypeBuilder<Auditorium> builder)
        {
            builder.OwnsMany(auditorium => auditorium.Seats, sb =>
            {
                sb.ToTable("AuditoriumsSeats");

                sb.WithOwner().HasForeignKey(nameof(AuditoriumId));

                sb.HasKey(nameof(Seat.Id), nameof(AuditoriumId));

                sb.Property(seat => seat.Id)
                .HasColumnName(nameof(Seat.Id))
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => new SeatId(value));

                sb.Property(seat => seat.Row)
                .HasConversion(
                    row => row.Value,
                    value => new Row(value));

                sb.Property(seat => seat.SeatNumber)
                .HasConversion(
                    seatNumber => seatNumber.Value,
                    value => new SeatNumber(value));
            });

            builder.Metadata.FindNavigation(nameof(Auditorium.Seats))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureAuditoriumsTable(EntityTypeBuilder<Auditorium> builder)
        {
            builder.ToTable("Auditoriums");

            base.Configure(builder);

            builder.Property(auditorium => auditorium.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => new AuditoriumId(value));

            builder.Property(auditorium => auditorium.Rows)
                .HasConversion(
                    rows => rows.Value,
                    value => new Rows(value));

            builder.Property(auditorium => auditorium.SeatsPerRow)
                .HasConversion(
                    seatsPerRow => seatsPerRow.Value,
                    value => new SeatsPerRow(value));
        }
    }
}