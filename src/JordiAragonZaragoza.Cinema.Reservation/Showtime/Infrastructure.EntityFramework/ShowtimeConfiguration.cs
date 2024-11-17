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
                  .HasConversion(ticketId => ticketId.Value, guidValue => new TicketId(guidValue));

                tb.Property(ticket => ticket.UserId)
                .HasConversion(userId => userId.Value, value => new UserId(value))
                .HasColumnName(nameof(UserId));

                tb.Property(ticket => ticket.ReservationDateOnUtc)
                    .HasConversion(reservationDate => reservationDate.Value, value => new ReservationDate(value))
                    .HasColumnName(nameof(ReservationDate));

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