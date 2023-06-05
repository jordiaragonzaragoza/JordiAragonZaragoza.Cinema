namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TicketSeatConfiguration : BaseEntityTypeConfiguration<TicketSeat, TicketSeatId>
    {
        public override void Configure(EntityTypeBuilder<TicketSeat> builder)
        {
            base.Configure(builder);

            builder.ToTable("TicketSeats");

            builder.Property(seat => seat.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => TicketSeatId.Create(value));
        }
    }
}