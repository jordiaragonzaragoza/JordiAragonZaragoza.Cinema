namespace JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// This configuration is obsolete when using showtime as event sourced aggregate.
    /// Its conserved as future reference.
    /// </summary>
    public sealed class ShowtimeConfiguration : BaseAggregateRootTypeConfiguration<Showtime, ShowtimeId>
    {
        public override void Configure(EntityTypeBuilder<Showtime> builder)
        {
            this.ConfigureShowtimesTable(builder);
            ConfigureShowtimesTicketsTable(builder);
        }

        private static void ConfigureShowtimesTicketsTable(EntityTypeBuilder<Showtime> builder)
        {
            builder.OwnsMany(showtime => showtime.Tickets, tb =>
            {
                tb.ToTable("ShowtimesTickets");

                tb.WithOwner().HasForeignKey(nameof(ShowtimeId));

                tb.HasKey(nameof(Ticket.Id), nameof(ShowtimeId));

                tb.Property(ticket => ticket.Id)
                  .HasColumnName(nameof(Ticket.Id))
                  .ValueGeneratedNever()
                  .HasConversion(ticketId => ticketId.Value, guidValue => TicketId.Create(guidValue));

                tb.Property(ticket => ticket.UserId)
                .HasConversion(id => id.Value, value => UserId.Create(value))
                .HasColumnName(nameof(UserId));

                tb.OwnsMany(ticket => ticket.Seats, ticketSeatBuilder =>
                {
                    ticketSeatBuilder.ToTable("ShowtimeTicketSeatIds");

                    ticketSeatBuilder.WithOwner().HasForeignKey(nameof(TicketId), nameof(ShowtimeId));

                    ticketSeatBuilder.Property(seatId => seatId.Value)
                        .HasColumnName(nameof(SeatId))
                        .ValueGeneratedNever();
                });

                tb.Navigation(x => x.Seats).Metadata.SetField("seats");
                tb.Navigation(x => x.Seats).UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            builder.Metadata.FindNavigation(nameof(Showtime.Tickets))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureShowtimesTable(EntityTypeBuilder<Showtime> builder)
        {
            builder.ToTable("Showtimes");

            base.Configure(builder);

            builder.Property(showtime => showtime.Id)
                .ValueGeneratedNever()
                .HasConversion(showtimeId => showtimeId.Value, intValue => ShowtimeId.Create(intValue));

            builder.Property(showtime => showtime.AuditoriumId)
                .HasConversion(id => id.Value, value => AuditoriumId.Create(value));

            builder.Property(showtime => showtime.MovieId)
                .HasConversion(id => id.Value, value => MovieId.Create(value));
        }
    }
}