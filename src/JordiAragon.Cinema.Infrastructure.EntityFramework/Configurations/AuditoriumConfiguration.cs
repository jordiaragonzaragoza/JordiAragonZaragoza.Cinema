namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuditoriumConfiguration : BaseEntityTypeConfiguration<Auditorium, AuditoriumId>
    {
        public override void Configure(EntityTypeBuilder<Auditorium> builder)
        {
            base.Configure(builder);

            builder.ToTable("Auditoriums");

            builder.Property(auditorium => auditorium.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => AuditoriumId.Create(value));

            builder.HasMany(auditorium => auditorium.Showtimes)
                .WithOne(showtime => showtime.Auditorium)
                .IsRequired();
        }
    }
}