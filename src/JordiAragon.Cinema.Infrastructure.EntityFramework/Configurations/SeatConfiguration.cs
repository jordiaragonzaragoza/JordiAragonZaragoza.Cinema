namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SeatConfiguration : BaseEntityTypeConfiguration<Seat, SeatId>
    {
        public override void Configure(EntityTypeBuilder<Seat> builder)
        {
            base.Configure(builder);

            builder.ToTable("Seats");

            builder.Property(seat => seat.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => SeatId.Create(value));

            builder.HasOne<Auditorium>()
                .WithMany(auditorium => auditorium.Seats)
                .HasForeignKey(seat => seat.AuditoriumId)
                .IsRequired();

            builder.HasMany(seat => seat.Tickets)
                    .WithOne()
                    .HasForeignKey(ticketSeat => ticketSeat.SeatId);
        }
    }
}