﻿namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
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
            ConfigureShowtimeTicketsTable(builder);
        }

        private static void ConfigureShowtimeTicketsTable(EntityTypeBuilder<Showtime> builder)
        {
            builder.OwnsMany(showtime => showtime.Tickets, tb =>
            {
                tb.ToTable("ShowtimeTickets");

                tb.WithOwner().HasForeignKey(nameof(ShowtimeId));

                tb.HasKey(nameof(Ticket.Id), nameof(ShowtimeId));

                tb.Property(ticket => ticket.Id)
                  ////.HasColumnName(nameof(TicketId))
                  .ValueGeneratedNever()
                  .HasConversion(ticketId => ticketId.Value, guidValue => TicketId.Create(guidValue));

                tb.OwnsMany(ticket => ticket.Seats, ticketSeatBuilder =>
                {
                    ticketSeatBuilder.ToTable("TicketSeatIds");

                    ticketSeatBuilder.WithOwner().HasForeignKey(nameof(TicketId), nameof(ShowtimeId));

                    ////ticketSeatBuilder.HasKey("Id");

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