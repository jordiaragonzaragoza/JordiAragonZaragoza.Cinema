namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuditoriumConfiguration : BaseEntityTypeConfiguration<Auditorium, AuditoriumId>
    {
        public override void Configure(EntityTypeBuilder<Auditorium> builder)
        {
            this.ConfigureAuditoriumsTable(builder);

            ConfigureAuditoriumShowtimeIdsTable(builder);

            ConfigureSeatsTable(builder);
        }

        private static void ConfigureAuditoriumShowtimeIdsTable(EntityTypeBuilder<Auditorium> builder)
        {
            builder.OwnsMany(auditorium => auditorium.Showtimes, sib =>
            {
                sib.ToTable("AuditoriumShowtimeIds");

                sib.WithOwner().HasForeignKey(nameof(AuditoriumId));

                sib.HasKey(nameof(Showtime.Id));

                sib.Property(showtimeId => showtimeId.Value)
                .ValueGeneratedNever()
                .HasColumnName(nameof(ShowtimeId));
            });

            builder.Metadata.FindNavigation(nameof(Auditorium.Showtimes))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private static void ConfigureSeatsTable(EntityTypeBuilder<Auditorium> builder)
        {
            builder.OwnsMany(auditorium => auditorium.Seats, sb =>
            {
                sb.ToTable("Seats");

                sb.WithOwner().HasForeignKey(nameof(AuditoriumId));

                sb.HasKey(nameof(Seat.Id), nameof(AuditoriumId));

                sb.Property(seat => seat.Id)
                .HasColumnName(nameof(SeatId))
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => SeatId.Create(value));
            });

            builder.Metadata.FindNavigation(nameof(Auditorium.Seats))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureAuditoriumsTable(EntityTypeBuilder<Auditorium> builder)
        {
            builder.ToTable("Auditoriums");

            base.Configure(builder);

            builder.Property(auditorium => auditorium.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => AuditoriumId.Create(value));
        }
    }
}