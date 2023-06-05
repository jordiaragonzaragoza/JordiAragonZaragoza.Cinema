namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TicketConfiguration : BaseEntityTypeConfiguration<Ticket, TicketId>
    {
        public override void Configure(EntityTypeBuilder<Ticket> builder)
        {
            base.Configure(builder);

            builder.ToTable("Tickets");

            builder.Property(ticket => ticket.Id)
                .ValueGeneratedNever()
                .HasConversion(ticketId => ticketId.Value, guidValue => TicketId.Create(guidValue));

            builder.HasMany(ticket => ticket.Seats)
                .WithOne()
                .HasForeignKey(ticketSeat => ticketSeat.TicketId)
                .IsRequired();
        }
    }
}