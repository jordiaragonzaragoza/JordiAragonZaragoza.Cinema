namespace JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using System;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class TicketReadModelConfiguration : BaseModelTypeConfiguration<TicketReadModel, Guid>
    {
        public override void Configure(EntityTypeBuilder<TicketReadModel> builder)
        {
            base.Configure(builder);

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