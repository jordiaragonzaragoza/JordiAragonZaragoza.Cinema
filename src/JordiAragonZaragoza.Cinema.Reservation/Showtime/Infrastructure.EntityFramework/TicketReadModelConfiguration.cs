namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class TicketReadModelConfiguration : BaseModelTypeConfiguration<TicketReadModel, Guid>
    {
        public override void Configure(EntityTypeBuilder<TicketReadModel> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

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

            builder.Metadata.FindNavigation(nameof(TicketReadModel.Seats))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}