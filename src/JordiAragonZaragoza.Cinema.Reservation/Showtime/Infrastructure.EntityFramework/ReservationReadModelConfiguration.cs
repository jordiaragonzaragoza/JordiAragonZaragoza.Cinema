namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class ReservationReadModelConfiguration : BaseModelTypeConfiguration<ReservationReadModel, Guid>
    {
        public override void Configure(EntityTypeBuilder<ReservationReadModel> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            base.Configure(builder);

            ConfigureReservationsSeatsTable(builder);
        }

        private static void ConfigureReservationsSeatsTable(EntityTypeBuilder<ReservationReadModel> builder)
        {
            builder.OwnsMany(reservation => reservation.Seats, sb =>
            {
                sb.ToTable("ReservationsSeats");

                sb.WithOwner().HasForeignKey(nameof(ReservationId));

                sb.HasKey(nameof(SeatReadModel.Id), nameof(ReservationId));
            });

            builder.Metadata.FindNavigation(nameof(ReservationReadModel.Seats))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}