namespace JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TicketReadModelConfiguration : IEntityTypeConfiguration<TicketReadModel>
    {
        public void Configure(EntityTypeBuilder<TicketReadModel> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(ticketReadModel => ticketReadModel.Id);

            ConfigureTicketsSeatsTable(builder);
        }

        private static void ConfigureTicketsSeatsTable(EntityTypeBuilder<TicketReadModel> builder)
        {
            builder.OwnsMany(ticket => ticket.Seats, sb =>
            {
                sb.ToTable("TicketsSeats");

                sb.WithOwner().HasForeignKey(nameof(TicketId));

                sb.HasKey(nameof(SeatReadModel.Id), nameof(TicketId));
            });

            builder.Metadata.FindNavigation(nameof(Ticket.Seats))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}