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

            this.ConfigureAuditoriumShowtimeIdsTable(builder);

            this.ConfigureSeatsTable(builder);
        }

        private void ConfigureAuditoriumsTable(EntityTypeBuilder<Auditorium> builder)
        {
            base.Configure(builder);

            builder.ToTable("Auditoriums");

            builder.Property(auditorium => auditorium.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => AuditoriumId.Create(value));
        }

        private void ConfigureAuditoriumShowtimeIdsTable(EntityTypeBuilder<Auditorium> builder)
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

        private void ConfigureSeatsTable(EntityTypeBuilder<Auditorium> builder)
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

                sb.Property(seat => seat.AuditoriumId)
                .HasConversion(id => id.Value, value => AuditoriumId.Create(value));
            });

            builder.Metadata.FindNavigation(nameof(Auditorium.Seats))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}