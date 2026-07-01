namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class AuditoriumReadModelConfiguration : BaseReadModelTypeConfiguration<AuditoriumReadModel, Guid, ReservationReadModelContext>
    {
        public AuditoriumReadModelConfiguration(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }

        public override void Configure(EntityTypeBuilder<AuditoriumReadModel> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            base.Configure(builder);

            ConfigureAuditoriumSeatsTable(builder);
        }

        private static void ConfigureAuditoriumSeatsTable(EntityTypeBuilder<AuditoriumReadModel> builder)
        {
            builder.OwnsMany(auditorium => auditorium.Seats, sb =>
            {
                sb.ToTable("AuditoriumSeats");

                sb.WithOwner().HasForeignKey(nameof(AuditoriumId));

                sb.HasKey(nameof(SeatReadModel.Id), nameof(AuditoriumId));
            });

            builder.Metadata.FindNavigation(nameof(AuditoriumReadModel.Seats))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}