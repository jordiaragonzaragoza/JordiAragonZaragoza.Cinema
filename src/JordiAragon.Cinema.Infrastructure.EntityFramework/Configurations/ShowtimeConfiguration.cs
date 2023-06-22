namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ShowtimeConfiguration : BaseEntityTypeConfiguration<Showtime, ShowtimeId>
    {
        public override void Configure(EntityTypeBuilder<Showtime> builder)
        {
            this.ConfigureShowtimesTable(builder);
            this.ConfigureTicketsTable(builder);
        }

        private void ConfigureShowtimesTable(EntityTypeBuilder<Showtime> builder)
        {
            base.Configure(builder);

            builder.ToTable("Showtimes");

            builder.Property(showtime => showtime.Id)
                .ValueGeneratedNever()
                .HasConversion(showtimeId => showtimeId.Value, intValue => ShowtimeId.Create(intValue));

            builder.Property(showtime => showtime.AuditoriumId)
                .HasConversion(id => id.Value, value => AuditoriumId.Create(value));

            builder.Property(showtime => showtime.MovieId)
                .HasConversion(id => id.Value, value => MovieId.Create(value));
        }

        private void ConfigureTicketsTable(EntityTypeBuilder<Showtime> builder)
        {
            builder.OwnsMany(showtime => showtime.Tickets, ticketBuilder =>
            {
                ticketBuilder.ToTable("Tickets");

                var x = ticketBuilder.WithOwner();
                x.HasForeignKey("ShowtimeId");

                ticketBuilder.HasKey(nameof(Ticket.Id), nameof(ShowtimeId));

                ticketBuilder.Property(ticket => ticket.Id)
                  .HasColumnName(nameof(TicketId))
                  .ValueGeneratedNever()
                  .HasConversion(ticketId => ticketId.Value, guidValue => TicketId.Create(guidValue));

                ticketBuilder.Property(ticket => ticket.ShowtimeId)
                  .HasConversion(id => id.Value, value => ShowtimeId.Create(value));

                ticketBuilder.OwnsMany(ticket => ticket.Seats, ticketSeatBuilder =>
                {
                    ticketSeatBuilder.ToTable("TicketSeatIds");

                    ticketSeatBuilder.WithOwner().HasForeignKey(nameof(TicketId), nameof(ShowtimeId));

                    ticketSeatBuilder.HasKey(nameof(Seat.Id));

                    ticketSeatBuilder.Property(seatId => seatId.Value)
                        .HasColumnName(nameof(SeatId))
                        .ValueGeneratedNever();
                });

                ticketBuilder.Navigation(x => x.Seats).Metadata.SetField("seats");
                ticketBuilder.Navigation(x => x.Seats).UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            builder.Metadata.FindNavigation(nameof(Showtime.Tickets))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}