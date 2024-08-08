namespace JordiAragon.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class AuditoriumConfiguration : BaseAggregateRootTypeConfiguration<Auditorium, AuditoriumId>
    {
        public override void Configure(EntityTypeBuilder<Auditorium> builder)
        {
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
                .HasConversion(id => id.Value, value => SeatId.Create(value));

                sb.Property(seat => seat.Row)
                .HasConversion(
                    row => row.Value,
                    value => Row.Create(value));

                sb.Property(seat => seat.SeatNumber)
                .HasConversion(
                    seatNumber => seatNumber.Value,
                    value => SeatNumber.Create(value));
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
                .HasConversion(id => id.Value, value => AuditoriumId.Create(value));

            builder.Property(auditorium => auditorium.Rows)
                .HasConversion(
                    rows => rows.Value,
                    value => Rows.Create(value));

            builder.Property(auditorium => auditorium.SeatsPerRow)
                .HasConversion(
                    seatsPerRow => seatsPerRow.Value,
                    value => SeatsPerRow.Create(value));
        }
    }
}